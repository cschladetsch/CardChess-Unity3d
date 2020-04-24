using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    //Arrow tip asset
    [CreateAssetMenu(fileName = "ArrowTip", menuName = "Cogobyte/ProceduralIndicators/Arrows/ArrowTip", order = 3)]
    [System.Serializable]
    public class ArrowTip : ScriptableObject
    {
        //References
        [System.NonSerialized]
        public ArrowPath arrowPath;
        //Editable Options
        //Path Options
        public Vector3 size = new Vector3(1, 1, 1);
        public enum ArrowTipMode { None, Mesh, Extrude }
        public ArrowTipMode arrowTipMode;
        [Range(1, 100)]
        public int levelOfDetailAlongPath = 1;
        public enum ArrowTipPathType { Function, FollowMainPath }
        public ArrowTipPathType arrowTipPathType;
        //Function Path
        public float pathAlongXFunctionLength = 1;
        public AnimationCurve pathAlongXFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public float pathAlongYFunctionLength = 1;
        public AnimationCurve pathAlongYFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public float pathAlongZFunctionLength = 1;
        public AnimationCurve pathAlongZFunction = AnimationCurve.Linear(0, 0, 1, 0);
        //Functions Scale Rotation And Shape
        public float widthFunctionLength = 1;
        public AnimationCurve widthFunction = AnimationCurve.Linear(0, 1, 1, 0);
        public float heightFunctionLength = 1;
        public AnimationCurve heightFunction = AnimationCurve.Linear(0, 1, 1, 0);
        public float rotationFunctionLength = 1;
        public AnimationCurve rotateFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public PrimitivesList templatePrimitives;
        public float shapeFunctionLength = 1;
        public AnimationCurve shapeFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public SolidMesh mesh;

        //Non editable - Do not change these fields by code
        public Matrix4x4[] extrudePoints;
        public int[] openClosed;
        public Primitive[] primitives;
        //Determines if it is tail(-1) or head(1)
        public int direction = 1;
        //Path calculated by function or by main path  if follow main path is on
        public List<Vector3> path;
        //Used to calculate maximum path length
        private float pathLength = 0;
        public Vector3 start = new Vector3(0, 0, 0);
        public Vector3 end = new Vector3(1, 1, 1);
        private static float errorRate = 0.0001f;
        public bool invalidTip = false;
        //propagate the rotation from path to continue on tip
        public float defaultRotateAngle = 0;

        void OnValidate()
        {
            size = new Vector3(Mathf.Max(size.x, 0), Mathf.Max(size.y, 0), Mathf.Max(size.z, 0));
            pathAlongXFunctionLength = Mathf.Max(pathAlongXFunctionLength, 1);
            pathAlongYFunctionLength = Mathf.Max(pathAlongYFunctionLength, 1);
            pathAlongZFunctionLength = Mathf.Max(pathAlongZFunctionLength, 1);
            ArrowPath.ValidateAnimationFunction(pathAlongXFunction, 0, 0);
            ArrowPath.ValidateAnimationFunction(pathAlongYFunction, 0, 0);
            ArrowPath.ValidateAnimationFunction(pathAlongZFunction, 0, 0);
            widthFunctionLength = Mathf.Max(widthFunctionLength, 1);
            heightFunctionLength = Mathf.Max(heightFunctionLength, 1);
            rotationFunctionLength = Mathf.Max(rotationFunctionLength, 1);
            shapeFunctionLength = Mathf.Max(shapeFunctionLength, 1);
            ArrowPath.ValidateAnimationFunction(widthFunction, 1, 0);
            ArrowPath.ValidateAnimationFunction(heightFunction, 1, 0);
            ArrowPath.ValidateAnimationFunction(rotateFunction, 0, 0);
            ArrowPath.ValidateAnimationFunction(shapeFunction, 0, 0);
        }

        //Returns the length of the tip
        public float getLength()
        {
            if (arrowTipMode == ArrowTipMode.None) return 0;
            return size.y;
        }

        //Should the extrude of tip be closed (extrude with faces), or open to continue on path extrude(extrude without faces)
        public bool getClosed()
        {
            return arrowPath.invalidPath == true || arrowTipMode == ArrowTipMode.None || (arrowPath.arrowPathMode != ArrowPath.ArrowPathMode.Extrude && arrowPath.arrowPathMode != ArrowPath.ArrowPathMode.BrokenExtrude);
        }

        //Caluclate tip path trajectory
        public void calculatePath()
        {
            if (arrowTipMode == ArrowTipMode.None) return;
            if (arrowTipPathType == ArrowTipPathType.FollowMainPath)
            {
                //path is caluclated by ArrowPath class, just calculate the length
                pathLength = 0;
                for (int i = 1; i < path.Count; i++)
                {
                    pathLength += (path[i] - path[i - 1]).magnitude;
                }
                return;
            }
            //Default values for all functions if not set
            OnValidate();
            float currentPercentage = 0;
            path = new List<Vector3>();
            //Set the first path point
            Vector3 tipDirection = end - start;
            Quaternion arrowTipRotation = Quaternion.FromToRotation(Vector3.up, tipDirection);
            path.Add(start + arrowTipRotation * new Vector3(pathAlongXFunction.Evaluate(0), pathAlongYFunction.Evaluate(0), pathAlongZFunction.Evaluate(0)));
            pathLength = 0;
            int[] currentKey = new int[3] { 0, 0, 0 };
            int currentKeyIndex = -1;
            float lastPathDifference = 0;
            //find the next Point
            increasePathPercentage(ref currentKeyIndex, ref currentPercentage, ref currentKey);
            while (true)
            {
                Vector3 pathItem;
                // add points until 100%
                if (currentPercentage >= 1.0f)
                {
                    //Put the last point exacly at 100%
                    pathItem = end + arrowTipRotation * new Vector3(pathAlongXFunction.Evaluate(pathAlongXFunctionLength), pathAlongYFunction.Evaluate(pathAlongYFunctionLength), pathAlongZFunction.Evaluate(pathAlongZFunctionLength));
                    lastPathDifference = (path[path.Count - 1] - pathItem).magnitude;
                    if (lastPathDifference > errorRate) path.Add(pathItem);
                    pathLength += lastPathDifference;
                    break;
                }
                pathItem = start + currentPercentage * tipDirection + arrowTipRotation * new Vector3(pathAlongXFunction.Evaluate(currentPercentage * pathAlongXFunctionLength), pathAlongYFunction.Evaluate(currentPercentage * pathAlongYFunctionLength), pathAlongZFunction.Evaluate(currentPercentage * pathAlongZFunctionLength));
                lastPathDifference = (path[path.Count - 1] - pathItem).magnitude;
                //never put the same path point twice
                if (lastPathDifference > errorRate) path.Add(pathItem);
                pathLength += lastPathDifference;
                increasePathPercentage(ref currentKeyIndex, ref currentPercentage, ref currentKey);
            }
        }

        //next point on path is either determined by level of detail or if there is a key on the pathfunctions, whichever is sooner on path
        private void increasePathPercentage(ref int currentKeyIndex, ref float currentPercentage, ref int[] currentKey)
        {
            currentKeyIndex = -1;
            currentPercentage += 1.0f / levelOfDetailAlongPath;
            if (currentKey[0] < pathAlongXFunction.keys.Length - 1 && pathAlongXFunction.keys[currentKey[0] + 1].time < currentPercentage * pathAlongXFunctionLength)
            {
                currentPercentage = pathAlongXFunction.keys[currentKey[0] + 1].time / pathAlongXFunctionLength;
                currentKeyIndex = 0;
            }
            if (currentKey[1] < pathAlongYFunction.keys.Length - 1 && pathAlongYFunction.keys[currentKey[1] + 1].time < currentPercentage * pathAlongYFunctionLength)
            {
                currentPercentage = pathAlongYFunction.keys[currentKey[1] + 1].time / pathAlongYFunctionLength;
                currentKeyIndex = 1;
            }
            if (currentKey[2] < pathAlongZFunction.keys.Length - 1 && pathAlongZFunction.keys[currentKey[2] + 1].time < currentPercentage * pathAlongZFunctionLength)
            {
                currentPercentage = pathAlongZFunction.keys[currentKey[2] + 1].time / pathAlongZFunctionLength;
                currentKeyIndex = 2;
            }
            if (currentKeyIndex != -1) currentKey[currentKeyIndex]++;
        }

        //Generate the extrude array
        public void generate()
        {
            if (arrowTipMode == ArrowTipMode.None) return;
            //set default functions if not set
            OnValidate();invalidTip = false;
            if (pathLength == 0){ invalidTip = true; return; }
            extrudePoints = new Matrix4x4[path.Count];
            primitives = new Primitive[path.Count];
            openClosed = new int[path.Count];
            float currentLength = 0;
            extrudePoints[0] = new Matrix4x4();
            //Calculate the first extrude
            Vector3 pathDiff = direction * (path[1] - path[0]);
            arrowPath.defaultRotateAngle = defaultRotateAngle;
            Quaternion nextDirectionX;
            Quaternion nextDirectionY;
            Quaternion extrudeDirectionX;
            Quaternion extrudeDirectionY;
            Quaternion lastDirectionX;
            Quaternion lastDirectionY;
            //First rotation depends on first path point direction and rotation from main path
            nextDirectionX = arrowPath.calculateUpDownQuaternion(pathDiff);
            nextDirectionY = arrowPath.calculateLeftRightQuaternion(pathDiff);
            lastDirectionX = nextDirectionX;
            lastDirectionY = nextDirectionY;
            float propagatingRotateAngle = 0;
            if (direction == -1)
            {
                propagatingRotateAngle = arrowPath.rotateFunction.Evaluate(0);
            }
            else
            {
                propagatingRotateAngle = arrowPath.rotateFunction.Evaluate(arrowPath.rotationFunctionLength);
            }
            extrudePoints[0].SetTRS(path[0], Quaternion.AngleAxis((rotateFunction.Evaluate(currentLength / pathLength * rotationFunctionLength) + propagatingRotateAngle) * 360, nextDirectionY * nextDirectionX * Vector2.up) * nextDirectionY * nextDirectionX, new Vector3(widthFunction.Evaluate(currentLength / pathLength * widthFunctionLength) * size.x, 1, heightFunction.Evaluate(currentLength / pathLength * heightFunctionLength) * size.z));
            //choose shape
            int primitiveIndex = (int)shapeFunction.Evaluate(0);
            if (primitiveIndex < 0) primitiveIndex = 0;
            if (primitiveIndex >= templatePrimitives.primitivesList.Count) primitiveIndex = templatePrimitives.primitivesList.Count - 1;
            primitives[0] = templatePrimitives.primitivesList[primitiveIndex];
            openClosed[0] = (getClosed()) ? 0 : 1;
            //Calculate the other extrude points
            for (int i = 1; i < path.Count; i++)
            {
                currentLength += (path[i] - path[i - 1]).magnitude;
                extrudePoints[i] = new Matrix4x4();
                float scaleWidthByAngle = 1f;
                float scaleHeightByAngle = 1f;
                if (i < path.Count - 1)
                {
                    pathDiff = direction * (path[i + 1] - path[i]);
                    nextDirectionX = arrowPath.calculateUpDownQuaternion(pathDiff);
                    nextDirectionY = arrowPath.calculateLeftRightQuaternion(pathDiff);
                    //Points depend on previous and next point directions
                    if (Quaternion.Angle(lastDirectionX, nextDirectionX) < 0.1f)
                    {
                        extrudeDirectionX = nextDirectionX;
                    }
                    else
                    {
                        extrudeDirectionX = Quaternion.Lerp(lastDirectionX, nextDirectionX, 0.5f);
                    }
                    if (Quaternion.Angle(lastDirectionY, nextDirectionY) < 0.1f)
                    {
                        extrudeDirectionY = nextDirectionY;
                    }
                    else
                    {
                        extrudeDirectionY = Quaternion.Lerp(lastDirectionY, nextDirectionY, 0.5f);
                    }
                    //Scale depends on angle between points
                    scaleWidthByAngle = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(lastDirectionY * Vector3.forward, nextDirectionY * Vector3.forward) / 2));
                    scaleWidthByAngle = (scaleWidthByAngle < 0.5f) ? 1 : 1 / scaleWidthByAngle;
                    scaleHeightByAngle = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(lastDirectionX * Vector3.up, nextDirectionX * Vector3.up) / 2));
                    scaleHeightByAngle = (scaleHeightByAngle < 0.5f) ? 1 : 1 / scaleHeightByAngle;
                }
                else
                {
                    pathDiff = direction * (path[i] - path[i - 1]);
                    nextDirectionX = arrowPath.calculateUpDownQuaternion(pathDiff);
                    nextDirectionY = arrowPath.calculateLeftRightQuaternion(pathDiff);
                    extrudeDirectionX = nextDirectionX;
                    extrudeDirectionY = nextDirectionY;
                }
                extrudePoints[i].SetTRS(path[i], Quaternion.AngleAxis((rotateFunction.Evaluate(currentLength / pathLength * rotationFunctionLength) + propagatingRotateAngle) * 360, extrudeDirectionY * extrudeDirectionX * Vector3.up) * extrudeDirectionY * extrudeDirectionX, new Vector3(scaleWidthByAngle * widthFunction.Evaluate(currentLength / pathLength * widthFunctionLength) * size.x, 1, scaleHeightByAngle * heightFunction.Evaluate(currentLength / pathLength * heightFunctionLength) * size.z));
                primitiveIndex = (int)(shapeFunction.Evaluate(currentLength / pathLength * shapeFunctionLength));
                if (primitiveIndex < 0) primitiveIndex = 0;
                if (primitiveIndex >= templatePrimitives.primitivesList.Count) primitiveIndex = templatePrimitives.primitivesList.Count - 1;
                primitives[i] = templatePrimitives.primitivesList[primitiveIndex];
                openClosed[i] = 1;
                lastDirectionX = nextDirectionX;
                lastDirectionY = nextDirectionY;
            }
            openClosed[path.Count - 1] = 2;
        }

        //Generates vertices and triangles for mesh tip
        public void generateMesh(List<Vector3> vertices, List<int> triangles, List<Vector3> normals)
        {
            //Generate default mesh if none is present
            if (mesh == null)
            {
                mesh = ScriptableObject.CreateInstance<SphereMesh>();
            }
            mesh.generate();
            //Calculate tip rotation
            Vector3 pathDiff = end - start;
            arrowPath.defaultRotateAngle = defaultRotateAngle;
            Quaternion nextDirectionX = arrowPath.calculateUpDownQuaternion(pathDiff);
            Quaternion nextDirectionY = arrowPath.calculateLeftRightQuaternion(pathDiff);
            float propagatingRotateAngle = 0;
            if (direction == -1)
            {
                propagatingRotateAngle = arrowPath.rotateFunction.Evaluate(0);
            }
            else
            {
                propagatingRotateAngle = arrowPath.rotateFunction.Evaluate(arrowPath.rotationFunctionLength);
            }
            //Calculate tip scale
            float meshBounds = 1 / mesh.mesh.bounds.max.y;
            Vector3 vertScale = meshBounds * size;
            //Generate each vertice and triangle
            Vector3[] tempVerts = mesh.verts;
            Vector3[] tempNormals = mesh.normals;
            int starterVerticeIndex = vertices.Count;
            for (int i = 0; i < tempVerts.Length; i++)
            {
                vertices.Add(start + Quaternion.AngleAxis(propagatingRotateAngle * 360, nextDirectionY * nextDirectionX * Vector3.up) * nextDirectionY * nextDirectionX * Vector3.Scale(vertScale, tempVerts[i]));
                normals.Add(Quaternion.AngleAxis(propagatingRotateAngle * 360, nextDirectionY * nextDirectionX * Vector3.up) * nextDirectionY * nextDirectionX * tempNormals[i]);
            }
            int[] tempTris = mesh.tris;
            for (int i = 0; i < tempTris.Length; i++)
            {
                triangles.Add(tempTris[i] + starterVerticeIndex);
            }
        }
    }
}