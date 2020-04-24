using Cogobyte.ProceduralIndicators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Special type of triangle that can be sliced by a plane and contains all triangle data including color
    public class MeshGridTriangle
    {
        //triangle vertices
        public int[] v = new int[3];
        //Sliced triangle data (first and second triangle if slice creates more than one triangle)
        public int triangleSide = 0;
        public MeshGridTriangle firstTris;
        public MeshGridTriangle secondTris;
        //Triangle color
        public Color32 triangleColor = Color.white;

        public MeshGridTriangle(int v1, int v2, int v3, Color32 color)
        {
            v[0] = v1;
            v[1] = v2;
            v[2] = v3;
            triangleColor = color;
        }

        //Slices the triangle into three default , firstTris and secondTris
        //Each slice can make a triangle and a quad that gets split into two triangles (firstTris and secondTris)
        public void SliceByPlane(Vector3 planePoint, Vector3 planeNormal, MeshGridIndicator meshGridIndicator)
        {
            triangleSide = 0;
            int[] sides = new int[3];
            sides[0] = CalculatePointSideOfPlane(meshGridIndicator.finalVertices[v[0]], planePoint, planeNormal);
            sides[1] = CalculatePointSideOfPlane(meshGridIndicator.finalVertices[v[1]], planePoint, planeNormal);
            sides[2] = CalculatePointSideOfPlane(meshGridIndicator.finalVertices[v[2]], planePoint, planeNormal);
            if (sides[0] == sides[1] && sides[0] == sides[2])
            {
                triangleSide = sides[0];
            }
            else if (sides[0] != sides[1] && sides[1] != sides[2] && sides[0] != sides[2])
            {
                int z = 0; int f = 1; int s = 2; int t;
                if (sides[1] == -1) { z = 1; f = 2; s = 0; }
                if (sides[2] == -1) { z = 2; f = 0; s = 1; }
                meshGridIndicator.finalVertices.Add(LinePlaneIntersection(meshGridIndicator.finalVertices[v[f]], meshGridIndicator.finalVertices[v[s]], planePoint, planeNormal));
                meshGridIndicator.finalNormals.Add((meshGridIndicator.finalNormals[v[f]] + meshGridIndicator.finalNormals[v[s]]) / 2);
                meshGridIndicator.finalUvs.Add((meshGridIndicator.finalUvs[v[f]] + meshGridIndicator.finalUvs[v[s]]) / 2);
                meshGridIndicator.finalColors.Add(meshGridIndicator.defaultColor);
                t = meshGridIndicator.finalVertices.Count - 1;
                int[] temp = new int[3] { v[0], v[1], v[2] };
                v[0] = temp[z]; v[1] = temp[f]; v[2] = t;
                AddNewTriangleVertex(meshGridIndicator, temp[z]);
                AddNewTriangleVertex(meshGridIndicator, t);
                firstTris = new MeshGridTriangle(temp[s], t + 1, t + 2, meshGridIndicator.defaultColor);
                firstTris.triangleSide = sides[s];
                triangleSide = sides[f];
            }
            else if (sides[0] == -1 || sides[1] == -1 || sides[2] == -1)
            {
                if (sides[0] == 1 || sides[1] == 1 || sides[2] == 1)
                {
                    triangleSide = 1;
                }
                else
                {
                    triangleSide = 0;
                }
            }
            else
            {
                int z = 0; int f = 1; int s = 2; int t; int t2;
                if (sides[1] == sides[2]) { z = 0; f = 1; s = 2; }
                if (sides[0] == sides[2]) { z = 1; f = 2; s = 0; }
                if (sides[1] == sides[0]) { z = 2; f = 0; s = 1; }
                meshGridIndicator.finalVertices.Add(LinePlaneIntersection(meshGridIndicator.finalVertices[v[z]], meshGridIndicator.finalVertices[v[f]], planePoint, planeNormal));
                t = meshGridIndicator.finalVertices.Count - 1;
                meshGridIndicator.finalNormals.Add((meshGridIndicator.finalNormals[v[z]] + meshGridIndicator.finalNormals[v[f]]) / 2);
                meshGridIndicator.finalUvs.Add((meshGridIndicator.finalUvs[v[z]] + meshGridIndicator.finalUvs[v[f]]) / 2);
                meshGridIndicator.finalColors.Add(meshGridIndicator.defaultColor);
                //second
                meshGridIndicator.finalVertices.Add(LinePlaneIntersection(meshGridIndicator.finalVertices[v[s]], meshGridIndicator.finalVertices[v[z]], planePoint, planeNormal));
                meshGridIndicator.finalNormals.Add((meshGridIndicator.finalNormals[v[s]] + meshGridIndicator.finalNormals[v[z]]) / 2);
                meshGridIndicator.finalUvs.Add((meshGridIndicator.finalUvs[v[s]] + meshGridIndicator.finalUvs[v[z]]) / 2);
                meshGridIndicator.finalColors.Add(meshGridIndicator.defaultColor);
                int[] temp = new int[3] { v[0], v[1], v[2] };
                AddNewTriangleVertex(meshGridIndicator, temp[s]);
                AddNewTriangleVertex(meshGridIndicator, t);
                firstTris = new MeshGridTriangle(temp[f], t + 2, t + 3, meshGridIndicator.defaultColor);
                firstTris.triangleSide = sides[s];
                AddNewTriangleVertex(meshGridIndicator, t);
                AddNewTriangleVertex(meshGridIndicator, t + 1);
                secondTris = new MeshGridTriangle(t + 4, temp[s], t + 5, meshGridIndicator.defaultColor);
                secondTris.triangleSide = sides[s];
                v[0] = temp[z]; v[1] = t; v[2] = t + 1;
                triangleSide = sides[z];
            }
        }

        //Adds the vertex into grid vertices used for the new sliced triangle 
        public void AddNewTriangleVertex(MeshGridIndicator meshGridIndicator, int v)
        {
            meshGridIndicator.finalVertices.Add(new Vector3(meshGridIndicator.finalVertices[v].x, meshGridIndicator.finalVertices[v].y, meshGridIndicator.finalVertices[v].z));
            meshGridIndicator.finalNormals.Add(meshGridIndicator.finalNormals[v]);
            meshGridIndicator.finalUvs.Add(meshGridIndicator.finalUvs[v]);
            meshGridIndicator.finalColors.Add(meshGridIndicator.defaultColor);
        }

        static float ErrorRate = 0.00001f;
        //Calculate which side of plane the point is on
        //-1 is that point lies on plane, 0 for normal side and 1 for counter normal side
        public static int CalculatePointSideOfPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
        {
            float val = Vector3.Dot(point, planeNormal) - Vector3.Dot(planePoint, planeNormal);
            if (Mathf.Abs(ErrorRate - val) < ErrorRate) return -1;
            if (val > 0) return 0;
            return 1;
        }

        //Function for intersection of line and plane
        public static Vector3 LinePlaneIntersection(Vector3 lineP1, Vector3 lineP2, Vector3 planePoint, Vector3 planeNormal)
        {
            float d = -Vector3.Dot(planePoint, planeNormal);
            float divider = Vector3.Dot(new Vector3(lineP2.x - lineP1.x, lineP2.y - lineP1.y, lineP2.z - lineP1.z), planeNormal);
            if (divider == 0) throw new UnityException("Parralel line plane");
            float factor = Vector3.Dot(lineP1, planeNormal) + d;
            return new Vector3(lineP1.x - (lineP2.x - lineP1.x) * factor / divider, lineP1.y - (lineP2.y - lineP1.y) * factor / divider, lineP1.z - (lineP2.z - lineP1.z) * factor / divider);
        }

    }
}