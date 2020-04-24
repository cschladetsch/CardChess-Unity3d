using Cogobyte.ProceduralLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Renders a SelectionIndicator as a full path mesh
    [CreateAssetMenu(fileName = "NormalSelectionIndicatorPath", menuName = "Cogobyte/ProceduralIndicators/Selection/NormalSelectionIndicatorPath", order = 1)]
    public class NormalSelectionIndicatorPath : SelectionIndicatorPath
    {
        public AnimationCurve outerWidthFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float outerWidthFunctionLength = 1f;
        public AnimationCurve innerWidthFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float innerWidthFunctionLength = 1f;
        public AnimationCurve heightFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float heightFunctionLength = 1f;
        public Gradient innerColor;
        public Gradient outerColor;
        public bool is3D = true;
        public bool flatShading = true;
        public AnimationCurve lowerBoundFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float lowerBoundFunctionLength = 1f;

        protected List<Vector3> lowerVertices;
        protected List<Vector3> vertices;
        protected List<int> triangles;
        protected List<Color32> vertColors;
        protected List<Vector3> upNormals;
        protected List<Vector3> sideNormals;

        public override void generate()
        {
            if (innerColor == null) prepareGradient(ref innerColor);
            if (outerColor == null) prepareGradient(ref outerColor);
            prepareOffset();
            mesh = new Mesh();
            doubleVerts = new List<int>();
            doubleVertsSecond = new List<int>();
            lowerVertices = new List<Vector3>();
            upNormals = new List<Vector3>();
            sideNormals = new List<Vector3>();
            vertices = new List<Vector3>();
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
                    GenerateBottomTriangles(pathArray.path.Count * 6);
                    GenerateSideTriangles(pathArray.path.Count * 2, pathArray.path.Count * 4);
                }
                else
                {
                    GenerateLowerVertsSmooth();
                    GenerateBottomTriangles(pathArray.path.Count * 2);
                    GenerateSideTriangles(0, pathArray.path.Count * 2);
                }
            }
            TransformOffsetVertices();
            verts = vertices.ToArray();
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
            mesh.name = "NormalSelectionPath";
        }

        public virtual void TransformOffsetVertices()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = position + Quaternion.Euler(rotation) * (Vector3.Scale(vertices[i] - pathArray.translation,scale)) + pathArray.translation;
            }
        }

        public virtual void TransformOffsetNormals()
        {
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = position + Quaternion.Euler(rotation) * (normals[i] - pathArray.translation) + pathArray.translation;
            }
        }

        public virtual void GenerateVertices()
        {
            List<Vector3> path = pathArray.path;
            Quaternion rot = Quaternion.Lerp(Quaternion.LookRotation(path[0] - path[path.Count - 2], Vector3.up), Quaternion.LookRotation(path[1] - path[0], Vector3.up), 0.5f);
            float currentPathLength = 0;
            vertices.Add(path[0] + outerWidthFunction.Evaluate(0) * (rot * Vector3.right) + heightFunction.Evaluate(0) * (rot * Vector3.up));
            upNormals.Add(rot * Vector3.up);
            sideNormals.Add(rot * Vector3.right);
            lowerVertices.Add(path[0] + outerWidthFunction.Evaluate(0) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(0) * (rot * Vector3.up));
            vertColors.Add(outerColor.Evaluate(0));
            vertices.Add(path[0] + innerWidthFunction.Evaluate(0) * (rot * Vector3.left) + heightFunction.Evaluate(0) * (rot * Vector3.up));
            upNormals.Add(rot * Vector3.up);
            sideNormals.Add(rot * Vector3.left);
            lowerVertices.Add(path[0] + innerWidthFunction.Evaluate(0) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(0) * (rot * Vector3.up));
            vertColors.Add(innerColor.Evaluate(0));
            for (int i = 1; i < path.Count - 1; i++)
            {
                currentPathLength += (path[i] - path[i - 1]).magnitude;
                rot = Quaternion.Lerp(Quaternion.LookRotation(path[i] - path[i - 1], Vector3.up), Quaternion.LookRotation(path[i + 1] - path[i], Vector3.up), 0.5f);
                vertices.Add(path[i] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                upNormals.Add(rot * Vector3.up);
                sideNormals.Add(rot * Vector3.right);
                lowerVertices.Add(path[i] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                vertColors.Add(outerColor.Evaluate(currentPathLength));
                vertices.Add(path[i] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                upNormals.Add(rot * Vector3.up);
                sideNormals.Add(rot * Vector3.left);
                lowerVertices.Add(path[i] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                vertColors.Add(innerColor.Evaluate(currentPathLength));
            }
            vertices.Add(vertices[0]);
            upNormals.Add(upNormals[0]);
            sideNormals.Add(sideNormals[0]);
            doubleVerts.Add(0);
            doubleVertsSecond.Add(vertices.Count - 1);
            lowerVertices.Add(lowerVertices[0]);
            vertColors.Add(outerColor.Evaluate(1));
            vertices.Add(vertices[1]);
            upNormals.Add(upNormals[1]);
            sideNormals.Add(sideNormals[1]);
            doubleVerts.Add(1);
            doubleVertsSecond.Add(vertices.Count - 1);
            lowerVertices.Add(lowerVertices[1]);
            vertColors.Add(innerColor.Evaluate(1));
        }

        public virtual void GenerateLowerVertsFlat()
        {
            int vertCount = vertices.Count;
            for (int i = 0; i < vertCount; i++)
            {
                vertices.Add(vertices[i]);
                vertColors.Add(vertColors[i]);
            }
            for (int i = 0; i < lowerVertices.Count; i++)
            {
                vertices.Add(lowerVertices[i]);
                vertColors.Add(vertColors[i]);
            }
            for (int i = 0; i < lowerVertices.Count; i++)
            {
                vertices.Add(lowerVertices[i]);
                vertColors.Add(vertColors[i]);
            }
        }

        public virtual void GenerateLowerVertsSmooth()
        {
            for (int i = 0; i < lowerVertices.Count; i++)
            {
                vertices.Add(lowerVertices[i]);
                vertColors.Add(vertColors[i]);
            }
            doubleVerts.Add(pathArray.path.Count);
            doubleVertsSecond.Add(pathArray.path.Count - 2);
            doubleVerts.Add(pathArray.path.Count + 1);
            doubleVertsSecond.Add(pathArray.path.Count - 1);
        }

        public virtual void GenerateTriangles()
        {
            for (int i = 0; i < pathArray.path.Count - 1; i++)
            {
                triangles.Add(i * 2);
                triangles.Add(i * 2 + 1);
                triangles.Add(i * 2 + 2);

                triangles.Add(i * 2 + 1);
                triangles.Add(i * 2 + 3);
                triangles.Add(i * 2 + 2);
            }
        }

        public virtual void GenerateSideTriangles(int startVertIndexFirst, int startVertIndexSecond)
        {
            for (int i = 0; i < pathArray.path.Count - 1; i++)
            {
                //innersidetris
                triangles.Add(startVertIndexFirst + i * 2);
                triangles.Add(startVertIndexSecond + i * 2 + 2);
                triangles.Add(startVertIndexSecond + i * 2);
                triangles.Add(startVertIndexFirst + i * 2);
                triangles.Add(startVertIndexFirst + i * 2 + 2);
                triangles.Add(startVertIndexSecond + i * 2 + 2);
                //RightSideTris
                triangles.Add(i * 2 + 1);
                triangles.Add(startVertIndexSecond + i * 2 + 1);
                triangles.Add(startVertIndexSecond + i * 2 + 3);
                triangles.Add(i * 2 + 1);
                triangles.Add(startVertIndexSecond + i * 2 + 3);
                triangles.Add(i * 2 + 3);
            }
        }

        public virtual void GenerateBottomTriangles(int startVertIndex)
        {
            for (int i = 0; i < pathArray.path.Count - 1; i++)
            {
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