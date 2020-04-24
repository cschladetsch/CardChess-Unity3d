using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    [CreateAssetMenu(fileName = "ArrowPath", menuName = "Cogobyte/ProceduralIndicators/Arrows/ArrowPath", order = 2)]
    [System.Serializable]
    public class ArrowPath : ScriptableObject
    {
        //References to tail and head
        [SerializeField]
        public ArrowTip arrowTail;
        [SerializeField]
        public ArrowTip arrowHead;
        //Options
        public enum ArrowPathMode { None, Extrude, MadeOfShapes, BrokenExtrude }
        public ArrowPathMode arrowPathMode;
        [Range(1, 1000)]
        public int levelOfDetailAlongPath = 1;
        public enum ArrowPathType { Function, PointArray , PathArray}
        public ArrowPathType arrowPathType;
        public PathArray pathArray;

        //Point Array Path
        public List<Vector3> editedPath;
        //Function Path
        public Vector3 startPoint = new Vector3(0, 0, 0);
        public Vector3 endPoint = new Vector3(10, 0, 0);
        public float pathAlongXFunctionLength = 1;
        public AnimationCurve pathAlongXFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public float pathAlongYFunctionLength = 1;
        public AnimationCurve pathAlongYFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public float pathAlongZFunctionLength = 1;
        public AnimationCurve pathAlongZFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public float widthFunctionLength = 1;
        public AnimationCurve widthFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float heightFunctionLength = 1;
        public AnimationCurve heightFunction = AnimationCurve.Linear(0, 0.1f, 1, 0.1f);
        public float rotationFunctionLength = 1;
        public AnimationCurve rotateFunction = AnimationCurve.Linear(0, 0, 1, 0);
        public PrimitivesList templatePrimitives;
        public float shapeFunctionLength = 1;
        public AnimationCurve shapeFunction = AnimationCurve.Linear(0, 0, 1, 0);
        //Made of Shapes Options
        public MeshesList customShapes;
        public bool atPathPoints = false;
        public List<float> distanceBetweenShapes = new List<float>();
        //Broken Extrude options
        public float brokenLineLength = 1;
        public float brakeLength = 1;
        //Personal class data
        //Extrude Data
        public Matrix4x4[] extrudePoints;
        public int[] openClosed;
        public Primitive[] primitives;
        //Calculated path data
        public Vector3[] path;
        public List<Vector3> fullPath;
        private float pathLength = 0;
        public Gradient[] colorFunctions;
        public bool useShapeColors;
        private static float errorRate = 0.0001f;
        public float defaultRotateAngle = 0;
        public List<Color32> madeOfShapesColors;
        public bool invalidPath = false;

        void OnValidate()
        {
            pathAlongXFunctionLength = Mathf.Max(pathAlongXFunctionLength, 1);
            pathAlongYFunctionLength = Mathf.Max(pathAlongYFunctionLength, 1);
            pathAlongZFunctionLength = Mathf.Max(pathAlongZFunctionLength, 1);
            ValidateAnimationFunction(pathAlongXFunction, 0, 0);
            ValidateAnimationFunction(pathAlongYFunction, 0, 0);
            ValidateAnimationFunction(pathAlongZFunction, 0, 0);
            widthFunctionLength = Mathf.Max(widthFunctionLength, 1);
            heightFunctionLength = Mathf.Max(heightFunctionLength, 1);
            rotationFunctionLength = Mathf.Max(rotationFunctionLength, 1);
            shapeFunctionLength = Mathf.Max(shapeFunctionLength, 1);
            ValidateAnimationFunction(widthFunction, 0.85f, 0.85f);
            ValidateAnimationFunction(heightFunction, 0.1f, 0.1f);
            ValidateAnimationFunction(rotateFunction, 0, 0);
            ValidateAnimationFunction(shapeFunction, 0, 0);
            brokenLineLength = Mathf.Max(brokenLineLength, 0.00001f);
            brakeLength = Mathf.Max(brakeLength, 0.00001f);
            ProceduralMesh.prepareColorGradient(ref colorFunctions);
            if(editedPath==null)
            {
                editedPath = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(10, 0, 0) };
            }
        }

        //Sets Default values on animation curves if not set
        public static void ValidateAnimationFunction(AnimationCurve curve, float startValue, float endValue)
        {
            if (curve == null)
            {
                curve = AnimationCurve.Linear(0, startValue, 1, endValue);
            }
            if (curve.length == 0)
            {
                curve.AddKey(0, startValue);
                curve.AddKey(1, endValue);
            }
            if (curve.postWrapMode == WrapMode.Default || curve.postWrapMode == WrapMode.Once)
            {
                curve.postWrapMode = WrapMode.Loop;
            }
        }

        public void CalculatePath()
        {
            float currentPercentage = 0;
            if (arrowPathType == ArrowPathType.PathArray && pathArray!=null)
            {
                pathArray.CalculatePath();
                fullPath = new List<Vector3>();
                fullPath.Add(pathArray.path[0]);
                for (int i=1;i< pathArray.path.Count; i++)
                {
                    if((pathArray.path[1]-pathArray.path[0]).magnitude>0.1) fullPath.Add(pathArray.path[i]);
                }
                fullPath = pathArray.path;
                pathLength = pathArray.maxPathLength;
                return;
            }
            fullPath = new List<Vector3>();
            OnValidate();
            //Uses the path array to create the path, but removes duplicates and adds aditional points for levelofdetail>1
            if (arrowPathType == ArrowPathType.PointArray)
            {
                pathLength = 0;
                Vector3 firstPathPoint = editedPath[0];
                Vector3 secondPathPoint;
                fullPath.Add(firstPathPoint);
                currentPercentage += 1.0f / levelOfDetailAlongPath;
                for (int i = 1; i < editedPath.Count; i++)
                {
                    if ((editedPath[i] - editedPath[i - 1]).magnitude < errorRate) continue;
                    secondPathPoint = editedPath[i];
                    while (true)
                    {
                        if (currentPercentage + 1.0f / levelOfDetailAlongPath >= 1.0f)
                        {
                            if ((secondPathPoint - firstPathPoint).magnitude > errorRate)
                            {
                                fullPath.Add(secondPathPoint);
                            }
                            currentPercentage = 1.0f / levelOfDetailAlongPath;
                            break;
                        }
                        if ((secondPathPoint - firstPathPoint).magnitude > errorRate)
                        {
                            fullPath.Add(firstPathPoint + currentPercentage * (secondPathPoint - firstPathPoint));
                        }
                        currentPercentage += 1.0f / levelOfDetailAlongPath;
                    }
                    pathLength += (secondPathPoint - firstPathPoint).magnitude;
                    firstPathPoint = secondPathPoint;
                }
                return;
            }
            //Sets default path function

            currentPercentage = 0;
            //Sets the first point
            Vector3 pointerDirection = endPoint - startPoint;
            fullPath.Add(startPoint + new Vector3(pathAlongXFunction.Evaluate(0), pathAlongYFunction.Evaluate(0), pathAlongZFunction.Evaluate(0)));
            pathLength = 0;
            int[] currentKey = new int[3] { 0, 0, 0 };
            int currentKeyIndex = -1;
            float lastPathDifference = 0;
            //Calculates next point 
            increasePathPercentage(ref currentKeyIndex, ref currentPercentage, ref currentKey);
            while (true)
            {
                Vector3 pathItem;
                if (currentPercentage >= 1.0f)
                {
                    pathItem = endPoint + new Vector3(pathAlongXFunction.Evaluate(pathAlongXFunctionLength), pathAlongYFunction.Evaluate(pathAlongYFunctionLength), pathAlongZFunction.Evaluate(pathAlongZFunctionLength));
                    lastPathDifference = (fullPath[fullPath.Count - 1] - pathItem).magnitude;
                    if (lastPathDifference > errorRate) fullPath.Add(pathItem);
                    pathLength += lastPathDifference;
                    break;
                }
                pathItem = startPoint + currentPercentage * pointerDirection + new Vector3(pathAlongXFunction.Evaluate(currentPercentage * pathAlongXFunctionLength), pathAlongYFunction.Evaluate(currentPercentage * pathAlongYFunctionLength), pathAlongZFunction.Evaluate(currentPercentage * pathAlongZFunctionLength));
                lastPathDifference = (fullPath[fullPath.Count - 1] - pathItem).magnitude;
                if (lastPathDifference > errorRate) fullPath.Add(pathItem);
                pathLength += lastPathDifference;
                increasePathPercentage(ref currentKeyIndex, ref currentPercentage, ref currentKey);
            }
        }

        //Calculates the point either by 1/level of detail or keyPoints on animation curves, whichever is sooner
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

        public void generate()
        {
            int extrudePathLength = path.Length;
            OnValidate();
            invalidPath = true;
            if (path.Length <= 1 || pathLength <= arrowHead.getLength() + arrowTail.getLength()) return;
            invalidPath = false;
            if (arrowPathMode == ArrowPathMode.BrokenExtrude)
            {
                //path of extrude
                List<Vector3> brokenPath = new List<Vector3>();
                //when the each broken line starts and ends
                List<int> brokenOpenClosePath = new List<int>();
                //iterator for path
                int currentPathIndex = 1;
                //distance traveled ?since last brokenPath point
                float currentDistance = 0;
                //is it currently in brokenLine or brake
                bool drawLine = true;
                //it will always start with line
                currentDistance = brokenLineLength;
                float magnitudeOfPathPart = (path[1] - path[0]).magnitude;
                //first point is at beggining of arrow tail
                brokenPath.Add(path[0]);
                brokenOpenClosePath.Add((arrowTail.getClosed()) ? 0 : 1);
                Vector3 currentShapeLocation = path[0];
                //until we exhaust all path points
                while (currentPathIndex < path.Length)
                {
                    //while we have points but still no ending for brokenline or brake
                    while (currentDistance >= magnitudeOfPathPart)
                    {
                        //add extrude point only if we have brokenline
                        if (drawLine)
                        {
                            if (brokenOpenClosePath[brokenOpenClosePath.Count - 1] == 2)
                            {
                                brokenPath.Add(path[currentPathIndex]);
                                brokenOpenClosePath.Add(0);
                            }
                            else
                            {
                                if ((path[currentPathIndex] - brokenPath[brokenPath.Count - 1]).magnitude > errorRate)
                                {
                                    brokenPath.Add(path[currentPathIndex]);
                                    brokenOpenClosePath.Add(1);
                                }
                            }
                        }
                        currentDistance -= magnitudeOfPathPart;
                        currentShapeLocation = path[currentPathIndex];
                        currentPathIndex++;
                        if (currentPathIndex >= path.Length) break;
                        magnitudeOfPathPart = (path[currentPathIndex] - path[currentPathIndex - 1]).magnitude;
                    }
                    if (currentPathIndex >= path.Length) break;
                    //switch line and brake in distance between two path points
                    while (currentDistance < magnitudeOfPathPart)
                    {
                        Vector3 bPathItem = currentShapeLocation + (path[currentPathIndex] - path[currentPathIndex - 1]).normalized * currentDistance;
                        //Never add a duplicate point
                        if ((bPathItem - brokenPath[brokenPath.Count - 1]).magnitude > errorRate)
                        {
                            brokenPath.Add(bPathItem);
                            if (drawLine)
                            {
                                brokenOpenClosePath.Add(2);
                                drawLine = !drawLine;
                                currentDistance += brakeLength;
                            }
                            else
                            {
                                brokenOpenClosePath.Add(0);
                                drawLine = !drawLine;
                                currentDistance += brokenLineLength;
                            }
                        }
                        else
                        {
                            if (drawLine)
                            {
                                brokenOpenClosePath[brokenPath.Count - 1] = 2;
                                drawLine = !drawLine;
                                currentDistance += brakeLength;
                            }
                            else
                            {
                                brokenOpenClosePath[brokenPath.Count - 1] = 0;
                                drawLine = !drawLine;
                                currentDistance += brokenLineLength;
                            }
                        }
                    }
                }
                if (drawLine)
                {
                    if (arrowHead.getClosed())
                    {
                        if ((brokenPath[brokenPath.Count - 1] - path[path.Length - 1]).magnitude > errorRate)
                        {
                            brokenPath.Add(path[path.Length - 1]);
                            brokenOpenClosePath.Add(2);
                        }
                        else
                        {
                            brokenOpenClosePath[brokenOpenClosePath.Count - 1] = 2;
                        }
                    }
                }
                else
                {
                    if (!arrowHead.getClosed())
                    {
                        if ((brokenPath[brokenPath.Count - 1] - path[path.Length - 1]).magnitude > errorRate)
                        {
                            brokenPath.Add(path[path.Length - 1]);
                            brokenOpenClosePath.Add(0);
                        }
                        else
                        {
                            brokenOpenClosePath[brokenOpenClosePath.Count - 1] = 0;
                        }
                    }
                }
                Extrude(brokenPath.ToArray());
                openClosed = brokenOpenClosePath.ToArray();
            }
            else
            {
                Extrude(path);
                openClosed[0] = (arrowTail.getClosed()) ? 0 : 1;
                for (int i = 1; i < path.Length; i++)
                {
                    openClosed[i] = 1;
                }
                openClosed[path.Length - 1] = (arrowHead.getClosed()) ? 2 : 1;
            }

        }

        //Main extrude method
        private void Extrude(Vector3[] path)
        {
            extrudePoints = new Matrix4x4[path.Length];
            primitives = new Primitive[path.Length];
            openClosed = new int[path.Length];
            float currentLength = 0;
            extrudePoints[0] = new Matrix4x4();
            Vector3 pathDiff = (path[1] - path[0]);
            defaultRotateAngle = 0;
            arrowTail.defaultRotateAngle = 0;
            //Calculate first point
            //First point rotation depends on first path point direction
            Quaternion lastDirectionX = calculateUpDownQuaternion(pathDiff);
            Quaternion lastDirectionY = calculateLeftRightQuaternion(pathDiff);
            extrudePoints[0].SetTRS(path[0], Quaternion.AngleAxis(rotateFunction.Evaluate(currentLength / pathLength * rotationFunctionLength) * 360, lastDirectionY * lastDirectionX * Vector2.up) * lastDirectionY * lastDirectionX, new Vector3(widthFunction.Evaluate(currentLength / pathLength * widthFunctionLength), 1, heightFunction.Evaluate(currentLength / pathLength * heightFunctionLength)));
            //choose shape to extrude
            int primitiveIndex = (int)(shapeFunction.Evaluate(0));
            if (primitiveIndex < 0) primitiveIndex = 0;
            if (primitiveIndex >= templatePrimitives.primitivesList.Count) primitiveIndex = templatePrimitives.primitivesList.Count - 1;
            primitives[0] = templatePrimitives.primitivesList[primitiveIndex];
            pathLength -= arrowHead.getLength() + arrowTail.getLength();
            //Calculate next extrude matrixes
            for (int i = 1; i < path.Length; i++)
            {
                currentLength += (path[i] - path[i - 1]).magnitude;
                extrudePoints[i] = new Matrix4x4();
                Quaternion nextDirectionX;
                Quaternion nextDirectionY;
                Quaternion extrudeDirectionX;
                Quaternion extrudeDirectionY;
                float scaleWidthByAngle = 1f;
                float scaleHeightByAngle = 1f;
                if (i < path.Length - 1)
                {
                    pathDiff = path[i + 1] - path[i];
                    nextDirectionX = calculateUpDownQuaternion(pathDiff);
                    nextDirectionY = calculateLeftRightQuaternion(pathDiff);
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
                    scaleWidthByAngle = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(lastDirectionY * Vector3.forward, nextDirectionY * Vector3.forward) / 2));
                    scaleWidthByAngle = (scaleWidthByAngle < 0.5f) ? 1 : 1 / scaleWidthByAngle;
                    scaleHeightByAngle = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(lastDirectionX * Vector3.up, lastDirectionX * Vector3.up) / 2));
                    scaleHeightByAngle = (scaleHeightByAngle < 0.5f) ? 1 : 1 / scaleHeightByAngle;
                }
                else
                {
                    pathDiff = path[i] - path[i - 1];
                    nextDirectionX = calculateUpDownQuaternion(pathDiff);
                    nextDirectionY = calculateLeftRightQuaternion(pathDiff);
                    extrudeDirectionX = nextDirectionX;
                    extrudeDirectionY = nextDirectionY;
                }
                extrudePoints[i].SetTRS(path[i], Quaternion.AngleAxis(rotateFunction.Evaluate(currentLength / pathLength * rotationFunctionLength) * 360, extrudeDirectionY * extrudeDirectionX * Vector3.up) * extrudeDirectionY * extrudeDirectionX, new Vector3(scaleWidthByAngle * widthFunction.Evaluate(currentLength / pathLength * widthFunctionLength), 1, scaleHeightByAngle * heightFunction.Evaluate(currentLength / pathLength * heightFunctionLength)));
                primitiveIndex = (int)(shapeFunction.Evaluate(currentLength / pathLength * shapeFunctionLength));
                if (primitiveIndex < 0) primitiveIndex = 0;
                if (primitiveIndex >= templatePrimitives.primitivesList.Count) primitiveIndex = templatePrimitives.primitivesList.Count - 1;
                primitives[i] = templatePrimitives.primitivesList[primitiveIndex];
                lastDirectionX = nextDirectionX;
                lastDirectionY = nextDirectionY;
            }
            arrowHead.defaultRotateAngle = defaultRotateAngle;
        }

        //Calculates the rotation part for up and down directions
        public Quaternion calculateUpDownQuaternion(Vector3 pathDiff)
        {
            if (pathDiff.y == 0)
            {
                return Quaternion.AngleAxis(90, Vector3.right);
            }
            return Quaternion.AngleAxis(Vector3.Angle(Vector3.up, pathDiff), Vector3.right);
        }

        //Calculates the rotation part for left or right rotation
        public Quaternion calculateLeftRightQuaternion(Vector3 pathDiff)
        {
            float rotateAroundY;
            if (pathDiff.x == 0 && pathDiff.z == 0)
            {
                rotateAroundY = defaultRotateAngle;
            }
            else
            {
                int rotationDirection = 1;
                if (pathDiff.x < 0) rotationDirection = -1;
                rotateAroundY = rotationDirection * Vector3.Angle(Vector3.forward, new Vector3(pathDiff.x, 0, pathDiff.z));
                defaultRotateAngle = rotateAroundY;
            }
            return Quaternion.AngleAxis(rotateAroundY, Vector3.up);
        }

        //Made of shapes mode, generates multiple shapes alon path
        public void GenerateMadeOfShapes(List<Vector3> vertices, List<int> triangles, List<Vector3> normals)
        {
            if (path.Length <= 1 || pathLength <= arrowHead.getLength() + arrowTail.getLength()) return;
            if (customShapes == null || customShapes.meshesList == null || customShapes.meshesList.Count == 0)
            {
                customShapes = ScriptableObject.CreateInstance<MeshesList>();
                customShapes.meshesList = new List<ProceduralMesh>();
                customShapes.meshesList.Add(ScriptableObject.CreateInstance<SphereMesh>());
            }
            //FIrst generate all shapes that will be used 
            for (int i = 0; i < customShapes.meshesList.Count; i++)
            {
                customShapes.meshesList[i].generate();
            }
            //Last point on path that was visited
            Vector3 currentShapeLocation = path[0];
            int shapeIndex = 0;
            int currentPathIndex = 1;
            //Whenver current distance passes the distanceBetweenShapes threshold add another shape to the mesh
            if (distanceBetweenShapes == null || distanceBetweenShapes.Count == 0)
            {
                distanceBetweenShapes = new List<float>();
            }
            while (distanceBetweenShapes.Count < customShapes.meshesList.Count)
            {
                distanceBetweenShapes.Add(1f);
            }
            if (atPathPoints)
            {
                distanceBetweenShapes = new List<float>();
                for(int i = 1; i < path.Length; i++)
                {
                    distanceBetweenShapes.Add((path[currentPathIndex] - path[currentPathIndex - 1]).magnitude);
                }
            }
            for (int i = 0; i < distanceBetweenShapes.Count; i++)
            {
                if (distanceBetweenShapes[i] < 0.05) distanceBetweenShapes[i] = 1f;
            }
            float currentDistance = distanceBetweenShapes[shapeIndex];
            float percentDistnace = 0;
            //Current used length for rotation function
            madeOfShapesColors = new List<Color32>();
            float currentLength = 0;
            shapeIndex++;
            shapeIndex = shapeIndex % customShapes.meshesList.Count;
            float magnitudeOfPathPart = (path[1] - path[0]).magnitude;
            //mesh.property is expensive save it somewher temporarly
            Vector3[][] tempVerts = new Vector3[customShapes.meshesList.Count][];
            int[][] tempTris = new int[customShapes.meshesList.Count][];
            Vector3[][] tempNormals = new Vector3[customShapes.meshesList.Count][];
            Color32[][] tempShapeColors = new Color32[customShapes.meshesList.Count][];
            for (int i = 0; i < customShapes.meshesList.Count; i++)
            {
                tempVerts[i] = customShapes.meshesList[i].verts;
                tempNormals[i] = customShapes.meshesList[i].normals;
                tempTris[i] = customShapes.meshesList[i].tris;
                tempShapeColors[i] = customShapes.meshesList[i].colors;
            }
            Vector3 pathDiff = (path[1] - path[0]);
            defaultRotateAngle = 0;
            arrowTail.defaultRotateAngle = 0;
            Quaternion lastDirectionX = calculateUpDownQuaternion(pathDiff);
            Quaternion lastDirectionY = calculateLeftRightQuaternion(pathDiff);
                //Calculate when the next mesh at percent of path part needs to be placed, for each path part
                while (currentPathIndex < path.Length)
                {
                    while (currentDistance > magnitudeOfPathPart)
                    {
                        currentLength += magnitudeOfPathPart;
                        currentShapeLocation = path[currentPathIndex];
                        currentDistance -= magnitudeOfPathPart;
                        currentPathIndex++;
                        if (currentPathIndex >= path.Length) break;
                        magnitudeOfPathPart = (path[currentPathIndex] - path[currentPathIndex - 1]).magnitude;
                        percentDistnace += magnitudeOfPathPart;
                        pathDiff = (path[currentPathIndex] - path[currentPathIndex - 1]);
                        defaultRotateAngle = 0;
                        arrowTail.defaultRotateAngle = 0;
                        lastDirectionX = calculateUpDownQuaternion(pathDiff);
                        lastDirectionY = calculateLeftRightQuaternion(pathDiff);
                    }
                    if (currentPathIndex >= path.Length) break;
                    while (currentDistance <= magnitudeOfPathPart)
                    {
                        int currentTriangleCount = vertices.Count;
                        currentLength += currentDistance;
                        percentDistnace += currentDistance;
                        for (int i = 0; i < tempVerts[shapeIndex].Length; i++)
                        {
                            vertices.Add(Quaternion.AngleAxis(rotateFunction.Evaluate(percentDistnace / pathLength * rotationFunctionLength) * 360, lastDirectionY * lastDirectionX * Vector2.up) * lastDirectionY * lastDirectionX * tempVerts[shapeIndex][i] + currentShapeLocation + (path[currentPathIndex] - path[currentPathIndex - 1]).normalized * currentDistance);
                            normals.Add(Quaternion.AngleAxis(rotateFunction.Evaluate(percentDistnace / pathLength * rotationFunctionLength) * 360, lastDirectionY * lastDirectionX * Vector2.up) * lastDirectionY * lastDirectionX * tempNormals[shapeIndex][i]);
                            if (useShapeColors)
                            {
                                madeOfShapesColors.Add(tempShapeColors[shapeIndex][i]);
                            }
                            else
                            {
                                madeOfShapesColors.Add(colorFunctions[0].Evaluate(percentDistnace / pathLength));
                            }
                        }
                        for (int i = 0; i < tempTris[shapeIndex].Length; i++)
                        {
                            triangles.Add(tempTris[shapeIndex][i] + currentTriangleCount);
                        }
                        currentDistance += distanceBetweenShapes[shapeIndex];
                        shapeIndex++;
                        shapeIndex = shapeIndex % customShapes.meshesList.Count;
                    }
                    currentShapeLocation = path[currentPathIndex];
                }

        }

        //Splits the main path into part for arrow tail, arrow path, and arrow head
        public void calculateArrowHeadStartPoints()
        {
            //lengths of both arrowMeshes
            float arrowTailRadius = arrowTail.getLength();
            float arrowHeadRadius = arrowHead.getLength();
            List<Vector3> calculatedPath = new List<Vector3>();
            arrowTail.start = arrowTail.end = fullPath[0];
            arrowHead.start = arrowHead.end = fullPath[fullPath.Count - 1];
            int pathBegin = 0;
            int pathEnd = fullPath.Count - 1;
            float distance;
            for (int i = 1; i < fullPath.Count; i++)
            {
                distance = Vector3.Distance(fullPath[i], fullPath[0]);
                if (Mathf.Abs(distance - arrowTailRadius) < 0.01)
                {
                    pathBegin = i;
                    arrowTail.start = fullPath[i];
                    break;
                }
                if (distance > arrowTailRadius)
                {
                    pathBegin = i;
                    arrowTail.start = LineSphereIntersection(fullPath[0], arrowTailRadius, fullPath[i - 1], fullPath[i])[0];
                    break;
                }
            }
            for (int i = fullPath.Count - 2; i >= 0; i--)
            {
                distance = Vector3.Distance(fullPath[i], fullPath[fullPath.Count - 1]);
                if (Mathf.Abs(distance - arrowHeadRadius) < 0.01)
                {
                    pathEnd = i;
                    arrowHead.start = fullPath[i];
                    break;
                }
                if (distance > arrowHeadRadius)
                {
                    pathEnd = i;
                    arrowHead.start = LineSphereIntersection(fullPath[fullPath.Count - 1], arrowHeadRadius, fullPath[i + 1], fullPath[i])[0];
                    break;
                }
            }
            arrowTail.path = new List<Vector3>();
            arrowTail.path.Add(arrowTail.start);
            for (int i = pathBegin - 1; i >= 0; i--)
            {
                arrowTail.path.Add(fullPath[i]);
                if (i != 0)
                {
                    for (int j = 1; j < arrowTail.levelOfDetailAlongPath; j++)
                    {
                        arrowTail.path.Add(fullPath[i] + j * 1f / levelOfDetailAlongPath * (fullPath[i - 1] - fullPath[i]));
                    }
                }
            }
            if (arrowTail.start != fullPath[pathBegin]) calculatedPath.Add(arrowTail.start);
            for (int i = pathBegin; i <= pathEnd; i++)
            {
                calculatedPath.Add(fullPath[i]);
            }
            if (arrowHead.start != fullPath[pathEnd]) calculatedPath.Add(arrowHead.start);
            arrowHead.path = new List<Vector3>();
            arrowHead.path.Add(arrowHead.start);
            for (int i = pathEnd + 1; i < fullPath.Count; i++)
            {
                arrowHead.path.Add(fullPath[i]);
                if (i != fullPath.Count - 1)
                {
                    for (int j = 1; j < arrowHead.levelOfDetailAlongPath; j++)
                    {
                        arrowHead.path.Add(fullPath[i] + j * 1f / levelOfDetailAlongPath * (fullPath[i + 1] - fullPath[i]));
                    }
                }
            }
            path = calculatedPath.ToArray();
        }


        //Calculates interection between spere and line
        public Vector3[] LineSphereIntersection(Vector3 center, float radius, Vector3 rayStart, Vector3 rayEnd)
        {
            Vector3 directionRay = rayEnd - rayStart;
            Vector3 centerToRayStart = rayStart - center;

            float a = Vector3.Dot(directionRay, directionRay);
            float b = 2 * Vector3.Dot(centerToRayStart, directionRay);
            float c = Vector3.Dot(centerToRayStart, centerToRayStart) - (radius * radius);

            float discriminant = (b * b) - (4 * a * c);
            if (discriminant >= 0)
            {
                //Ray did not miss
                discriminant = Mathf.Sqrt(discriminant);

                //How far on ray the intersections happen
                float t1 = (-b - discriminant) / (2 * a);
                float t2 = (-b + discriminant) / (2 * a);

                Vector3[] hitPoints;

                if (t1 >= 0 && t2 >= 0)
                {
                    //total intersection, return both points
                    hitPoints = new Vector3[2];
                    hitPoints[0] = rayStart + (directionRay * t1);
                    hitPoints[1] = rayStart + (directionRay * t2);
                }
                else
                {
                    //Only one intersected, return one point
                    hitPoints = new Vector3[1];
                    if (t1 >= 0)
                    {
                        hitPoints[0] = rayStart + (directionRay * t1);
                    }
                    else if (t2 >= 0)
                    {
                        hitPoints[0] = rayStart + (directionRay * t2);
                    }
                }
                return hitPoints;
            }
            //No hits
            return null;
        }
    }
}