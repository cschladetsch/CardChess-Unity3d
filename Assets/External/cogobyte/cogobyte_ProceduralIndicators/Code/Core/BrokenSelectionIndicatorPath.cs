using Cogobyte.ProceduralLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Renders a SelectionIndicator as a broken path mesh
    [CreateAssetMenu(fileName = "BrokenSelectionIndicatorPath", menuName = "Cogobyte/ProceduralIndicators/Selection/BrokenSelectionIndicatorPath", order = 3)]
    public class BrokenSelectionIndicatorPath : NormalSelectionIndicatorPath
    {
        [Range(0.00001f, 1000)]
        public int numberOfBrokenLines = 1;
        [Range(0f, 1f)]
        public float percentageOfEmptySpace = 0.1f;

        protected List<int> brokenOpenClosePath;

        public override void generate()
        {
            if (innerColor == null) prepareGradient(ref innerColor);
            if (outerColor == null) prepareGradient(ref outerColor);
            prepareOffset();
            mesh = new Mesh();
            lowerVertices = new List<Vector3>();
            vertices = new List<Vector3>();
            upNormals = new List<Vector3>();
            sideNormals = new List<Vector3>();
            triangles = new List<int>();
            vertColors = new List<Color32>();
            GenerateVertices();
            if (pathArray.obstacleCheck == PathArray.ObstacleCheckMode.Parellel)
            {
                pathArray.ParralelObstacleCheck(vertices);
            }
            else if (pathArray.obstacleCheck == PathArray.ObstacleCheckMode.Projection)
            {
                pathArray.ProjectionObstacleCheck(vertices);
            }
            GenerateTriangles();
            if (is3D)
            {
                if (flatShading)
                {
                    GenerateLowerVertsFlat();
                    GenerateBottomTriangles(brokenOpenClosePath.Count * 2);
                    GenerateSideTriangles(brokenOpenClosePath.Count * 2, brokenOpenClosePath.Count * 4);
                }
                else
                {
                    GenerateLowerVertsSmooth();
                    GenerateBottomTriangles(brokenOpenClosePath.Count * 2);
                    GenerateSideTriangles(0, brokenOpenClosePath.Count * 2);
                }
                int startVertIndex = brokenOpenClosePath.Count * 2;
                
            }
            TransformOffsetVertices();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.uv = uvs;
            mesh.SetColors(vertColors);
            if (flatShading)
            {
                if (is3D)
                {
                    normals = new Vector3[upNormals.Count * 4];
                    for (int i = 0; i < upNormals.Count; i++)
                    {
                        normals[i] = upNormals[i];
                        normals[upNormals.Count + i] = sideNormals[i];
                        normals[upNormals.Count * 2 + i] = sideNormals[i];
                        normals[upNormals.Count * 3 + i] = -upNormals[i];
                    }
                }
                else
                {
                    normals = upNormals.ToArray();
                }
            }
            else
            {
                FixNormals();
            }
            TransformOffsetNormals();
            mesh.normals = normals;
            mesh.name = "BrokenSelectionPath";
        }



        public override void GenerateVertices()
        {
            List<Vector3> path = pathArray.path;
            float brokenLineLength = pathArray.maxPathLength / numberOfBrokenLines;
            float brakeDistance = percentageOfEmptySpace * brokenLineLength;
            brokenLineLength -= brakeDistance;
            //iterator for path
            int currentPathIndex = 1;
            //distance traveled ?since last brokenPath point
            float currentDistance = 0;
            //is it currently in brokenLine or brake
            bool drawStatus = false;
            //it will always start with line
            currentDistance = brakeDistance;
            float magnitudeOfPathPart = (path[1] - path[0]).magnitude;
            Quaternion rot = Quaternion.Lerp(Quaternion.LookRotation(path[0] - path[path.Count - 2], Vector3.up), Quaternion.LookRotation(path[1] - path[0], Vector3.up), 0.5f);
            float currentPathLength = 0;

            brokenOpenClosePath = new List<int>();
            Vector3 currentShapeLocation = path[0];

            while (currentPathIndex < path.Count)
            {
                //while we have points but still no ending for brokenline or brake
                while (currentDistance >= magnitudeOfPathPart)
                {
                    //add extrude point only if we have brokenline
                    if (drawStatus)
                    {
                        vertices.Add(path[currentPathIndex] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        upNormals.Add(rot * Vector3.up);
                        sideNormals.Add(rot * Vector3.right);
                        lowerVertices.Add(path[currentPathIndex] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        vertColors.Add(outerColor.Evaluate(currentPathLength));
                        vertices.Add(path[currentPathIndex] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        upNormals.Add(rot * Vector3.up);
                        sideNormals.Add(rot * Vector3.left);
                        lowerVertices.Add(path[currentPathIndex] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        vertColors.Add(innerColor.Evaluate(currentPathLength));
                        if (brokenOpenClosePath.Count == 0 || brokenOpenClosePath[brokenOpenClosePath.Count - 1] == 2)
                        {
                            brokenOpenClosePath.Add(0);
                        }
                        else
                        {
                            brokenOpenClosePath.Add(1);
                        }
                    }
                    currentPathLength += magnitudeOfPathPart;
                    currentDistance -= magnitudeOfPathPart;
                    currentShapeLocation = path[currentPathIndex];
                    currentPathIndex++;
                    if (currentPathIndex >= path.Count) break;
                    magnitudeOfPathPart = (path[currentPathIndex] - path[currentPathIndex - 1]).magnitude;
                    if (currentPathIndex < path.Count - 1)
                        rot = Quaternion.Lerp(Quaternion.LookRotation(path[currentPathIndex] - path[currentPathIndex - 1], Vector3.up), Quaternion.LookRotation(path[currentPathIndex + 1] - path[currentPathIndex], Vector3.up), 0.5f);
                }
                if (currentPathIndex >= path.Count) break;
                //switch line and brake in distance between two path points
                while (currentDistance < magnitudeOfPathPart)
                {
                    rot = Quaternion.LookRotation((path[currentPathIndex] - path[currentPathIndex - 1]).normalized, Vector3.up);
                    currentPathLength += currentDistance;
                    Vector3 bPathItem = currentShapeLocation + (path[currentPathIndex] - path[currentPathIndex - 1]).normalized * currentDistance;
                    vertices.Add(bPathItem + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    upNormals.Add(rot * Vector3.up);
                    sideNormals.Add(rot * Vector3.right);
                    lowerVertices.Add(bPathItem + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    vertColors.Add(outerColor.Evaluate(currentPathLength));
                    vertices.Add(bPathItem + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    upNormals.Add(rot * Vector3.up);
                    sideNormals.Add(rot * Vector3.right);
                    lowerVertices.Add(bPathItem + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    vertColors.Add(innerColor.Evaluate(currentPathLength));

                    if (drawStatus)
                    {
                        brokenOpenClosePath.Add(2);
                        drawStatus = !drawStatus;
                        currentDistance += brakeDistance;
                    }
                    else
                    {
                        brokenOpenClosePath.Add(0);
                        drawStatus = !drawStatus;
                        currentDistance += brokenLineLength;
                    }
                }
            }
        }

        public override void GenerateLowerVertsSmooth()
        {
            for (int i = 0; i < lowerVertices.Count; i++)
            {
                vertices.Add(lowerVertices[i]);
                vertColors.Add(vertColors[i]);
            }
        }

        public override void GenerateTriangles()
        {
            for (int i = 0; i < brokenOpenClosePath.Count - 1; i++)
            {
                if (brokenOpenClosePath[i] != 2)
                {
                    triangles.Add(i * 2);
                    triangles.Add(i * 2 + 1);
                    triangles.Add(i * 2 + 2);

                    triangles.Add(i * 2 + 1);
                    triangles.Add(i * 2 + 3);
                    triangles.Add(i * 2 + 2);
                }
            }
        }

        public override void GenerateSideTriangles(int startVertIndexFirst, int startVertIndexSecond)
        {
            for (int i = 0; i < brokenOpenClosePath.Count - 1; i++)
            {
                if (brokenOpenClosePath[i] != 2)
                {

                    //innersidetris

                    triangles.Add(startVertIndexFirst + i * 2);
                    triangles.Add(startVertIndexSecond + i * 2 + 2);
                    triangles.Add(startVertIndexSecond + i * 2);

                    triangles.Add(startVertIndexFirst + i * 2);
                    triangles.Add(startVertIndexFirst + i * 2 + 2);
                    triangles.Add(startVertIndexSecond + i * 2 + 2);
                    //RightSideTris
                    triangles.Add(startVertIndexFirst + i * 2 + 1);
                    triangles.Add(startVertIndexSecond + i * 2 + 1);
                    triangles.Add(startVertIndexSecond + i * 2 + 3);

                    triangles.Add(startVertIndexFirst + i * 2 + 1);
                    triangles.Add(startVertIndexSecond + i * 2 + 3);
                    triangles.Add(startVertIndexFirst + i * 2 + 3);
                    if (brokenOpenClosePath[i] == 0)
                    {
                        triangles.Add(startVertIndexSecond + i * 2);
                        triangles.Add(startVertIndexSecond + i * 2 + 1);
                        triangles.Add(startVertIndexFirst + i * 2 + 1);
                        //RightSideTris
                        triangles.Add(startVertIndexSecond + i * 2);
                        triangles.Add(startVertIndexFirst + i * 2 + 1);
                        triangles.Add(startVertIndexFirst + i * 2);
                    }
                }
                else
                {
                    triangles.Add(startVertIndexSecond + i * 2);
                    triangles.Add(startVertIndexFirst + i * 2 + 1);
                    triangles.Add(startVertIndexSecond + i * 2 + 1);
                    //RightSideTris
                    triangles.Add(startVertIndexSecond + i * 2);
                    triangles.Add(startVertIndexFirst + i * 2);
                    triangles.Add(startVertIndexFirst + i * 2 + 1);
                }

            }
        }

        public override void GenerateBottomTriangles(int startVertIndex)
        {
            for (int i = 0; i < brokenOpenClosePath.Count - 1; i++)
            {
                if (brokenOpenClosePath[i] != 2)
                {
                    //lower tris
                    triangles.Add(startVertIndex + i * 2);
                    triangles.Add(startVertIndex + i * 2 + 2);
                    triangles.Add(startVertIndex + i * 2 + 1);

                    triangles.Add(startVertIndex + i * 2 + 1);
                    triangles.Add(startVertIndex + i * 2 + 2);
                    triangles.Add(startVertIndex + i * 2 + 3);
                }
            }
        }

    }
}