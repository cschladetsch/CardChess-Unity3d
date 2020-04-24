using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Redners a mesh sliced by an 3d grid  
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshGridIndicator : MonoBehaviour
    {
        //Grid length by each cell by each axis, array size determines number of cells
        public float[] gridX = new float[1] { 1 };
        public float[] gridY = new float[1] { 1 };
        public float[] gridZ = new float[1] { 1 };
        //Default color of each cell
        public Color32 defaultColor = Color.green;
        //Color of mesh if it is outside of the grid
        public Color32 outsideColor = Color.red;
        //Offset of the whole grid. First cell starts at mesh point (0,0,0)
        public Vector3 gridOffset = new Vector3(0,0,0);
        //Mesh that is sliced by the grid
        public Mesh mesh;
        //Transform of the mesh
        public Vector3 meshPosition = Vector3.zero;
        public Vector3 meshRotation = Vector3.zero;
        public Vector3 meshScale = Vector3.one;

        //Sliced tringles data organised in a 3d array
        Queue<MeshGridTriangle>[,,] triangleSeparation;
        Queue<MeshGridTriangle> outside;
        //Mesh data
        [System.NonSerialized]
        public List<Vector3> finalVertices;
        [System.NonSerialized]
        public List<Vector3> finalNormals;
        [System.NonSerialized]
        public List<Vector2> finalUvs;
        [System.NonSerialized]
        public List<Color32> finalColors;
        List<int> finalTriangles;
        Mesh m;

        public void Start()
        {
            if (gridX.Length > 0 && gridY.Length > 0 && gridZ.Length > 0 && mesh!=null)
            {
                MakeSeparation();
            }
        }

        //Sets the color of the triangles inside x,y,z grid cell
        public void SetColor(int x, int y, int z, Color32 color)
        {
            //if there are triangles in the cell
            if (triangleSeparation[x, y, z] != null)
            {
                int numberOfTris = triangleSeparation[x, y, z].Count;
                for (int p = 0; p < numberOfTris; p++)
                {
                    MeshGridTriangle t = triangleSeparation[x, y, z].Dequeue();
                    t.triangleColor = color;
                    finalColors[t.v[0]] = color;
                    finalColors[t.v[1]] = color;
                    finalColors[t.v[2]] = color;
                    triangleSeparation[x, y, z].Enqueue(t);
                }
                m.SetColors(finalColors);
            }
        }

        //Resets all triangles to default colors
        public void ResetColor()
        {
            for(int i = 0; i < finalColors.Count; i++)
            {
                finalColors[i] = defaultColor;
            }
            m.SetColors(finalColors);
        }

        //Generates the sliced mesh with triangles organized in 3d grid
        public void MakeSeparation()
        {
            if (m == null) m = new Mesh();
            outside = null;
            triangleSeparation = new Queue<MeshGridTriangle>[gridX.Length, gridY.Length, gridZ.Length];
            triangleSeparation[0, 0, 0] = new Queue<MeshGridTriangle>();
            int[] tris = mesh.triangles;
            Vector3[] verts = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] uvs = mesh.uv;
            finalVertices = new List<Vector3>();
            finalNormals = new List<Vector3>();
            finalUvs = new List<Vector2>();
            finalColors = new List<Color32>();
            finalTriangles = new List<int>();
            //Convert mesh triangles into MeshGridTriangle triangle
            for (int i = 0; i < tris.Length; i += 3)
            {
                triangleSeparation[0, 0, 0].Enqueue(new MeshGridTriangle(i, i + 1,i + 2,defaultColor));
                finalVertices.Add(meshPosition + Quaternion.Euler(meshRotation) * Vector3.Scale(meshScale, verts[tris[i]]));
                finalNormals.Add(Quaternion.Euler(meshRotation) * normals[tris[i]]);
                finalUvs.Add(uvs[tris[i]]);
                finalColors.Add(defaultColor);
                //second
                finalVertices.Add(meshPosition + Quaternion.Euler(meshRotation) * Vector3.Scale(meshScale, verts[tris[i+1]]));
                finalNormals.Add(Quaternion.Euler(meshRotation) * normals[tris[i+1]]);
                finalUvs.Add(uvs[tris[i+1]]);
                finalColors.Add(defaultColor);
                //third
                finalVertices.Add(meshPosition + Quaternion.Euler(meshRotation) * Vector3.Scale(meshScale, verts[tris[i+2]]));
                finalNormals.Add(Quaternion.Euler(meshRotation) * normals[tris[i+2]]);
                finalUvs.Add(uvs[tris[i+2]]);
                finalColors.Add(defaultColor);
            }
            //Seperate the triangles that are outside of the grid
            SeperateTriangleZones(gridOffset, Vector3.left, ref triangleSeparation[0, 0, 0], ref outside);
            SeperateTriangleZones(gridOffset, Vector3.down, ref triangleSeparation[0, 0, 0], ref outside);
            SeperateTriangleZones(gridOffset, Vector3.back, ref triangleSeparation[0, 0, 0], ref outside);

            Vector3 planePoint = gridOffset;
            float distanceX = 0;
            float distanceY = 0;
            float distanceZ = 0;
            //Split triangles into each cell
            for (int i = 0; i < gridX.Length; i++)
            {
                distanceY = 0;
                distanceX += gridX[i];
                if (triangleSeparation[i, 0, 0] != null)
                {
                    if (i != gridX.Length - 1)
                    {
                        SeperateTriangleZones(gridOffset + new Vector3(distanceX, 0, 0), Vector3.right, ref triangleSeparation[i, 0, 0], ref triangleSeparation[i + 1, 0, 0]);
                    }
                    else
                    {
                        SeperateTriangleZones(gridOffset + new Vector3(distanceX, 0, 0), Vector3.right, ref triangleSeparation[i, 0, 0], ref outside);
                    }
                    distanceY = 0;
                    for (int j = 0; j < gridY.Length; j++)
                    {
                        distanceY += gridY[j];
                        if (triangleSeparation[i, j, 0] != null)
                        {
                            if (j != gridY.Length - 1)
                            {
                                SeperateTriangleZones(gridOffset + new Vector3(0, distanceY, 0), Vector3.up, ref triangleSeparation[i, j, 0], ref triangleSeparation[i, j + 1, 0]);
                            }
                            else
                            {
                                SeperateTriangleZones(gridOffset + new Vector3(0, distanceY, 0), Vector3.up, ref triangleSeparation[i, j, 0], ref outside);
                            }
                            distanceZ = 0;
                            for(int k = 0; k < gridZ.Length; k++)
                            {
                                distanceZ += gridZ[k];
                                if (triangleSeparation[i, j, k] != null)
                                {
                                    if (k != gridZ.Length - 1)
                                    {
                                        SeperateTriangleZones(gridOffset + new Vector3(0, 0, distanceZ), Vector3.forward, ref triangleSeparation[i, j, k], ref triangleSeparation[i, j, k+1]);
                                    }
                                    else
                                    {
                                        SeperateTriangleZones(gridOffset + new Vector3(0, 0, distanceZ), Vector3.forward, ref triangleSeparation[i, j, k], ref outside);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Set color of outside triangles
            if (outside != null)
            {
                int numberOfTris = outside.Count;
                for (int p = 0; p < numberOfTris; p++)
                {
                    MeshGridTriangle t = outside.Dequeue();
                    t.triangleColor = outsideColor;
                    finalColors[t.v[0]] = outsideColor;
                    finalColors[t.v[1]] = outsideColor;
                    finalColors[t.v[2]] = outsideColor;
                    finalTriangles.Add(t.v[0]);
                    finalTriangles.Add(t.v[1]);
                    finalTriangles.Add(t.v[2]);
                    outside.Enqueue(t);
                }
            }
            //Set color and triangles in each cell
            for (int i = 0; i < gridX.Length; i++)
            {
                for (int j = 0; j < gridY.Length; j++) {
                    for(int k = 0; k < gridZ.Length; k++)
                    {
                        if (triangleSeparation[i, j, k] != null)
                        {
                            int numberOfTris = triangleSeparation[i, j, k].Count;
                            for (int p = 0; p < numberOfTris; p++)
                            {
                                MeshGridTriangle t = triangleSeparation[i, j, k].Dequeue();
                                finalTriangles.Add(t.v[0]);
                                finalTriangles.Add(t.v[1]);
                                finalTriangles.Add(t.v[2]);
                                triangleSeparation[i, j, k].Enqueue(t);
                            }
                        }
                    }
                }
            }
            //Set the mesh
            m.Clear();
            m.SetVertices(finalVertices);
            m.SetUVs(0, finalUvs);
            m.SetTriangles(finalTriangles, 0);
            m.SetColors(finalColors);
            m.RecalculateBounds();
            m.RecalculateNormals();
            m.SetNormals(finalNormals);
            m.UploadMeshData(false);
            GetComponent<MeshFilter>().mesh = m;
        }

        //Slices the mesh triangles into the next cell
        private void SeperateTriangleZones(Vector3 planePoint,Vector3 planeNormal,ref Queue<MeshGridTriangle> mainZone, ref Queue<MeshGridTriangle> other)
        {
            int numberOfTris = mainZone.Count;
            for (int j = 0; j < numberOfTris; j++)
            {
                MeshGridTriangle triangle = mainZone.Dequeue();
                triangle.SliceByPlane(planePoint, planeNormal, this);
                if (triangle.triangleSide != 0) { if (mainZone == null) mainZone = new Queue<MeshGridTriangle>(); mainZone.Enqueue(triangle); }
                else { if (other == null) other = new Queue<MeshGridTriangle>(); other.Enqueue(triangle); }
                if (triangle.firstTris != null)
                {
                    if (triangle.firstTris.triangleSide != 0) { if (mainZone == null) mainZone = new Queue<MeshGridTriangle>(); mainZone.Enqueue(triangle.firstTris); triangle.firstTris = null; }
                    else { if (other == null) other = new Queue<MeshGridTriangle>(); other.Enqueue(triangle.firstTris); triangle.firstTris = null; }
                }
                if (triangle.secondTris != null)
                {
                    if (triangle.secondTris.triangleSide != 0) { if (mainZone == null) mainZone = new Queue<MeshGridTriangle>(); mainZone.Enqueue(triangle.secondTris); triangle.secondTris = null; }
                    else { if (other == null) other = new Queue<MeshGridTriangle>(); other.Enqueue(triangle.secondTris); triangle.secondTris = null; }
                }
            }
        }


    }
}
