using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    //Renders a SelectionIndicator as a list of meshes along path
    [CreateAssetMenu(fileName = "MadeOfShapesSelectionIndicatorPath", menuName = "Cogobyte/ProceduralIndicators/Selection/MadeOfShapesSelectionIndicatorPath", order = 2)]
    public class MadeOfShapesSelectionIndicatorPath : SelectionIndicatorPath
    {
        //Shapes to use (repeats)
        public MeshesList customShapes;
        //Space allocated for each shape
        public List<float> SpaceTakenByShape = new List<float>();
        //Use shape colors or color function
        public bool useShapeColors = true;
        //Gradient along path to use colors
        public Gradient colorFunction;

        private List<Color32> madeOfShapesColors;
        public override void generate()
        {
            if (colorFunction == null) prepareGradient(ref colorFunction);
            List<Vector3> path = pathArray.path;
            prepareOffset();
            mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            if (customShapes == null || customShapes.meshesList == null || customShapes.meshesList.Count == 0)
            {
                customShapes = ScriptableObject.CreateInstance<MeshesList>();
                customShapes.meshesList = new List<ProceduralMesh>();
                customShapes.meshesList.Add(ScriptableObject.CreateInstance<SphereMesh>());
            }
            for (int i = 0; i < customShapes.meshesList.Count; i++)
            {
                customShapes.meshesList[i].generate();
            }
            //Whenver current distance passes the distanceBetweenShapes threshold add another shape to the mesh
            if (SpaceTakenByShape == null || SpaceTakenByShape.Count == 0)
            {
                SpaceTakenByShape = new List<float>();
            }
            while (SpaceTakenByShape.Count < customShapes.meshesList.Count)
            {
                SpaceTakenByShape.Add(1f);
            }
            for (int i = 0; i < SpaceTakenByShape.Count; i++)
            {
                if (SpaceTakenByShape[i] < 0.05) SpaceTakenByShape[i] = 1f;
            }
            float shapesMaxDistance = 0;
            for (int i = 0; i < SpaceTakenByShape.Count; i++)
            {
                shapesMaxDistance += SpaceTakenByShape[i];
            }
            float pathLength = 0;
            for (int i = 1; i < path.Count; i++)
            {
                pathLength += (path[i] - path[i - 1]).magnitude;
            }
            int numberOfShapeInstances = Mathf.FloorToInt((pathLength / shapesMaxDistance));
            float maxBrakeDistance = ((pathLength / shapesMaxDistance) - numberOfShapeInstances) * shapesMaxDistance;
            float brakeDistance = maxBrakeDistance / (numberOfShapeInstances * customShapes.meshesList.Count);
            //Last point on path that was visited
            Vector3 currentShapeLocation = path[0];
            int shapeIndex = 0;
            int lastShapeIndex = 0;
            int currentPathIndex = 1;
            float currentDistance = SpaceTakenByShape[shapeIndex] / 2;
            float percentDistnace = 0;
            madeOfShapesColors = new List<Color32>();
            float currentLength = 0;
            float magnitudeOfPathPart = (path[1] - path[0]).magnitude;
            //mesh.property is expensive save it somewher temporarly
            Vector3[][] tempVerts = new Vector3[customShapes.meshesList.Count][];
            int[][] tempTris = new int[customShapes.meshesList.Count][];
            Color32[][] tempShapeColors = new Color32[customShapes.meshesList.Count][];
            for (int i = 0; i < customShapes.meshesList.Count; i++)
            {
                tempVerts[i] = customShapes.meshesList[i].verts;
                tempTris[i] = customShapes.meshesList[i].tris;
                tempShapeColors[i] = customShapes.meshesList[i].colors;
            }
            //Calculate when the next mesh at percent of path part needs to be placed, for each path part
            while (currentPathIndex < path.Count)
            {
                while (currentDistance > magnitudeOfPathPart)
                {
                    currentLength += magnitudeOfPathPart;
                    currentShapeLocation = path[currentPathIndex];
                    currentDistance -= magnitudeOfPathPart;
                    currentPathIndex++;
                    if (currentPathIndex >= path.Count) break;
                    magnitudeOfPathPart = (path[currentPathIndex] - path[currentPathIndex - 1]).magnitude;
                    percentDistnace += magnitudeOfPathPart;
                }
                if (currentPathIndex >= path.Count) break;
                while (currentDistance <= magnitudeOfPathPart)
                {
                    int currentTriangleCount = vertices.Count;
                    currentLength += currentDistance;
                    percentDistnace += currentDistance;
                    for (int i = 0; i < tempVerts[shapeIndex].Length; i++)
                    {
                        vertices.Add(tempVerts[shapeIndex][i] + currentShapeLocation + (path[currentPathIndex] - path[currentPathIndex - 1]).normalized * currentDistance);
                        if (useShapeColors)
                        {
                            madeOfShapesColors.Add(tempShapeColors[shapeIndex][i]);
                        }
                        else
                        {
                            madeOfShapesColors.Add(colorFunction.Evaluate(percentDistnace / pathLength));
                        }
                    }
                    for (int i = 0; i < tempTris[shapeIndex].Length; i++)
                    {
                        triangles.Add(tempTris[shapeIndex][i] + currentTriangleCount);
                    }
                    currentDistance += SpaceTakenByShape[lastShapeIndex] / 2 + SpaceTakenByShape[shapeIndex] / 2 + brakeDistance;
                    lastShapeIndex = shapeIndex;
                    shapeIndex++;
                    shapeIndex = shapeIndex % customShapes.meshesList.Count;
                }
                currentShapeLocation = path[currentPathIndex];
            }
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.uv = uvs;
            mesh.SetColors(madeOfShapesColors);
            mesh.RecalculateNormals();
            mesh.name = "MadeOfShapesSelectionPath";
        }
    }
}