using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Redners an array size X*Y*Z grid with default color and thickness
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GridIndicator : MonoBehaviour
    {
        //Grid length by each cell by each axis, array size determines number of cells
        public float[] gridX = new float[1] { 1 };
        public float[] gridY = new float[1] { 1 };
        public float[] gridZ = new float[1] { 1 };
        //Default Color of the grid
        public Color32 gridColor = Color.green;
        //Offset of the whole grid. First cell starts at mesh point (0,0,0)
        public Vector3 gridOffset = new Vector3(0,0,0);
        //Thickness of the frid wire
        public float thickness = 0.2f;
        //mesh data
        [System.NonSerialized]
        public List<Vector3> finalVertices;
        [System.NonSerialized]
        public List<Vector3> finalNormals;
        [System.NonSerialized]
        public List<Vector2> finalUvs;
        [System.NonSerialized]
        public List<Color32> finalColors;
        List<int> finalTriangles;
        [System.NonSerialized]
        public Mesh m;
        int vertexCount = 0;

        void Start()
        {
            if (gridX.Length > 0 && gridY.Length > 0 && gridZ.Length > 0)
            {
                Generate();
            }
        }

        //Changes the whole grid color
        public void SetGridColor(Color32 newColor)
        {
            for (int i = 0; i < finalVertices.Count; i++)
            {
                finalColors[i] = newColor;
            }
            m.SetColors(finalColors);
            GetComponent<MeshFilter>().mesh = m;
        }

        //Generates the grid mesh
        public void Generate()
        {
            //Mesh initialization
            if (m == null) m = new Mesh();
            finalVertices = new List<Vector3>();
            finalNormals = new List<Vector3>();
            finalColors = new List<Color32>();
            finalTriangles = new List<int>();
            vertexCount = 0;
            //Generate 6 sides of the grid
            GenerateGridPart(gridZ, gridX, gridY, thickness / 2, 0, 2, 1, 0);
            GenerateGridPart(gridZ, gridX, gridY, -thickness / 2, 1, 2, 1, 0);
            GenerateGridPart(gridX, gridZ, gridY, thickness / 2, 1, 0, 1, 2);
            GenerateGridPart(gridX, gridZ, gridY, -thickness / 2, 0, 0, 1, 2);
            GenerateGridPart(gridY, gridZ, gridX, thickness / 2, 0, 1, 0, 2);
            GenerateGridPart(gridY, gridZ, gridX, -thickness / 2, 1, 1, 0, 2);
            //Offset the grid by grid offset
            for (int i = 0; i < finalVertices.Count; i++)
            {
                finalColors.Add(gridColor);
                finalVertices[i] += gridOffset;
            }
            //Set the mesh
            m.Clear();
            m.SetVertices(finalVertices);
            m.SetTriangles(finalTriangles, 0);
            m.SetColors(finalColors);
            m.RecalculateBounds();
            m.RecalculateNormals();
            GetComponent<MeshFilter>().mesh = m;
        }

        //Generates vertices and triangles for each side of the grid (6 sides)
        private void GenerateGridPart(float[] firstGrid, float[] secondGrid, float[] thirdGrid, float firstBorder, int side, int X, int Y, int Z)
        {
            float[] temp = new float[3] { 0, 0, 0 };
            float secondDist = 0;
            int a; int b;
            float thirdDist = 0;
            float firstDist = 0;
            float border = thickness / 2;
            for (int k = 0; k <= firstGrid.Length; k++)
            {
                secondDist = 0;
                for (int i = 0; i <= secondGrid.Length; i++)
                {
                    thirdDist = 0;
                    for (int j = 0; j <= thirdGrid.Length; j++)
                    {
                        temp[2] = secondDist - border; temp[1] = thirdDist - border; temp[0] = firstDist - firstBorder;
                        finalVertices.Add(new Vector3(temp[X], temp[Y], temp[Z]));
                        temp[2] = secondDist - border; temp[1] = thirdDist + border; temp[0] = firstDist - firstBorder;
                        finalVertices.Add(new Vector3(temp[X], temp[Y], temp[Z]));
                        temp[2] = secondDist + border; temp[1] = thirdDist - border; temp[0] = firstDist - firstBorder;
                        finalVertices.Add(new Vector3(temp[X], temp[Y], temp[Z]));
                        temp[2] = secondDist + border; temp[1] = thirdDist + border; temp[0] = firstDist - firstBorder;
                        finalVertices.Add(new Vector3(temp[X], temp[Y], temp[Z]));
                        vertexCount += 4;
                        if (k == 0 || (k == firstGrid.Length))
                        {
                            finalTriangles.Add(vertexCount - 4);
                            a = vertexCount - 3; b = vertexCount - 2;
                            if (side == 0)
                            {
                                finalTriangles.Add(a);
                                finalTriangles.Add(b);
                            }
                            else
                            {
                                finalTriangles.Add(b);
                                finalTriangles.Add(a);
                            }
                            finalTriangles.Add(vertexCount - 3);
                            a = vertexCount - 1; b = vertexCount - 2;
                            if (side == 0)
                            {
                                finalTriangles.Add(a);
                                finalTriangles.Add(b);
                            }
                            else
                            {
                                finalTriangles.Add(b);
                                finalTriangles.Add(a);
                            }
                        }
                        if (j < thirdGrid.Length)
                        {
                            finalTriangles.Add(vertexCount - 1);
                            a = vertexCount; b = vertexCount + 2;
                            if (side == 0)
                            {
                                finalTriangles.Add(a);
                                finalTriangles.Add(b);
                            }
                            else
                            {
                                finalTriangles.Add(b);
                                finalTriangles.Add(a);
                            }
                            finalTriangles.Add(vertexCount - 3);
                            a = vertexCount; b = vertexCount - 1;
                            if (side == 0)
                            {
                                finalTriangles.Add(a);
                                finalTriangles.Add(b);
                            }
                            else
                            {
                                finalTriangles.Add(b);
                                finalTriangles.Add(a);
                            }

                        }
                        if (i < secondGrid.Length)
                        {
                            finalTriangles.Add(vertexCount - 1);
                            a = vertexCount + thirdGrid.Length * 4; b = vertexCount - 2;
                            if (side == 0)
                            {
                                finalTriangles.Add(a);
                                finalTriangles.Add(b);
                            }
                            else
                            {
                                finalTriangles.Add(b);
                                finalTriangles.Add(a);
                            }
                            finalTriangles.Add(vertexCount - 1);
                            a = vertexCount + thirdGrid.Length * 4 + 1; b = vertexCount + thirdGrid.Length * 4;
                            if (side == 0)
                            {
                                finalTriangles.Add(a);
                                finalTriangles.Add(b);
                            }
                            else
                            {
                                finalTriangles.Add(b);
                                finalTriangles.Add(a);
                            }

                        }
                        if (j < thirdGrid.Length) thirdDist += thirdGrid[j];
                    }
                    if (i < secondGrid.Length) secondDist += secondGrid[i];
                }
                if (k < firstGrid.Length) firstDist += firstGrid[k];
            }
        }
    }
}