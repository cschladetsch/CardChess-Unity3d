using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Custom poly shape that can be edited in unity editor (when selected)
    [CreateAssetMenu(fileName = "CustomPolyPrimitive", menuName = "Cogobyte/ProceduralLibrary/Primitives/CustomPolyPrimitive", order = 4)]
    [System.Serializable]
    public class CustomPolyPrimitive : Primitive
    {
        //Points used as borders
        public List<Vector3> points = new List<Vector3>() { new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0) };
        
        //Coloring from first to last point, uses only the zero gradient in array
        public Gradient[] gradient;

        //Used for ear clipping algorithm
        private LinkedList<Vertex> unusedPoints;

        public override int getVertexCount()
        {
            if (twoSided) return 2 * points.Count;
            return points.Count;
        }

        public override int getTrisCount()
        {
            if(twoSided) return 6 * (points.Count - 1);
            return 3*(points.Count-1);
        }


        public void OnValidate()
        {
            if (points.Count == 0)
            {
                points.Add(new Vector3(-1, 0, 0));
                points.Add(new Vector3(0, 0, 1));
                points.Add(new Vector3(1, 0, 0));
            }
            if (points.Count == 1)
            {
                points.Add(points[0] + new Vector3(-1, 0, -1));
                points.Add(points[0] + new Vector3(1, 0, -1));
            }
            if (points.Count == 2)
            {
                points.Add(points[0] + Quaternion.Euler(new Vector3(0, 60, 0)) * (points[1] - points[0]));
            }
            if (points.Count == 3)
            {
                if (IsConvex(points[0], points[1], points[2]))
                {
                    Vector3 temp = points[1];
                    points[1] = points[2];
                    points[2] = temp;
                }
            }
            prepareColorGradient(ref gradient);
            for (int i = 0; i < points.Count; i++)
            {
                int lastPoint = (i == 0) ? points.Count - 1 : i - 1;
                int nextPoint = (i == points.Count - 1) ? 0 : i + 1;
                if (IsConvex(points[lastPoint], points[i], points[nextPoint]))
                {
                    for (int j = 0; j < points.Count; j++)
                    {
                        int nextJ = (j == points.Count - 1) ?0: j+1;
                        if (j != i && j!=lastPoint) {
                            if (CalculatePointSideOfPlane(points[i], points[j], (Vector3.Cross(points[nextJ] - points[j],Vector3.up)).normalized)==1)
                            {
                                if (Mathf.Abs(Vector3.Angle(points[nextPoint] - points[i], points[nextJ] - points[j])) > ErrorRate &&
                                    Mathf.Abs(Vector3.Angle(points[i] - points[lastPoint], points[nextJ] - points[j])) > ErrorRate
                                    )
                                {
                                    Vector3 crossPoint1 = LinePlaneIntersection(points[i], points[nextPoint], points[j], (Vector3.Cross(points[nextJ] - points[j], Vector3.up)).normalized);
                                    Vector3 crossPoint2 = LinePlaneIntersection(points[i] , points[lastPoint], points[j], (Vector3.Cross(points[nextJ] - points[j], Vector3.up)).normalized);
                                    if (CalculatePointSideOfPlane(crossPoint1,points[j],points[j]-points[nextJ])== 1
                                        && CalculatePointSideOfPlane(crossPoint1, points[nextJ], points[nextJ] - points[j]) == 1
                                        &&
                                        CalculatePointSideOfPlane(crossPoint2, points[j], points[j] - points[nextJ]) == 1
                                        && CalculatePointSideOfPlane(crossPoint2, points[nextJ], points[nextJ] - points[j]) == 1
                                        ) {
                                        Debug.DrawLine(crossPoint1, crossPoint1 + Vector3.up * 7, Color.red, 20);
                                        Debug.DrawLine(crossPoint2, crossPoint2 + Vector3.up * 7, Color.red, 20);
                                        Debug.DrawLine(points[j] - 7 * (points[nextJ] - points[j]), points[j] + 7 * (points[nextJ] - points[j]), Color.green, 20);
                                        Debug.DrawLine(points[i] - 7 * (points[lastPoint] - points[i]), points[i] + 7 * (points[lastPoint] - points[i]), Color.blue, 20);
                                        Debug.DrawLine(points[i] - 7 * (points[nextPoint] - points[i]), points[i] + 7 * (points[nextPoint] - points[i]), Color.blue, 20);
                                        Debug.DrawLine(points[j], points[j] + 8 *(Vector3.Cross(points[nextJ] - points[j], Vector3.up)).normalized, Color.red, 20);
                                        points[i] = (points[lastPoint] + points[nextPoint]) / 2;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        public static float ErrorRate = 0.00001f;


        //Calculate which side of plane the point is on
        //-1 is that point lies on plane, 0 for normal side and 1 for counter normal side
        public static int CalculatePointSideOfPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
        {
            float val = Vector3.Dot(point, planeNormal) - Vector3.Dot(planePoint, planeNormal);
            if (Mathf.Abs(ErrorRate - val) < ErrorRate) return -1;
            if (val > 0) return 0;
            return 1;
        }

        public static Vector3 LinePlaneIntersection(Vector3 lineP1, Vector3 lineP2, Vector3 planePoint, Vector3 planeNormal)
        {
            float d = -Vector3.Dot(planePoint, planeNormal);
            float divider = Vector3.Dot(new Vector3(lineP2.x - lineP1.x, lineP2.y - lineP1.y, lineP2.z - lineP1.z), planeNormal);
            if (divider == 0) throw new UnityException("Parralel line plane");
            float factor = Vector3.Dot(lineP1, planeNormal) + d;
            return new Vector3(lineP1.x - (lineP2.x - lineP1.x) * factor / divider, lineP1.y - (lineP2.y - lineP1.y) * factor / divider, lineP1.z - (lineP2.z - lineP1.z) * factor / divider);
        }

        public override void generate()
        {
            base.generate();
            prepareColorGradient(ref gradient);
            normals = new Vector3[getVertexCount()];
            int triangleIndex = 0;
            int vertexIndex = 0;
            verts[vertexIndex] = transformOffset.MultiplyPoint(points[0]);
            normals[vertexIndex] = transformOffset.MultiplyVector(Vector3.up);
            colors[vertexIndex] = gradient[0].Evaluate(0);
            vertexIndex++;
            unusedPoints = new LinkedList<Vertex>();
            unusedPoints.AddFirst(new Vertex(points[0],0));
            LinkedListNode<Vertex> currentNode = unusedPoints.First;
            Vector3 min = points[0];
            Vector3 max = points[0];
            for(int i = 1; i < points.Count; i++)
            {
                verts[vertexIndex] = transformOffset.MultiplyPoint(points[i]);
                normals[vertexIndex] = transformOffset.MultiplyVector(Vector3.up);
                colors[vertexIndex] = gradient[0].Evaluate((float)i / points.Count);
                vertexIndex++;
                unusedPoints.AddAfter(currentNode, new Vertex(points[i],i));
                currentNode = currentNode.Next;
                if (points[i].x > max.x) max.x = points[i].x;
                if (points[i].x < min.x) min.x = points[i].x;
                if (points[i].z > max.z) max.z = points[i].z;
                if (points[i].z < min.z) min.z = points[i].z;
            }
            for (int i = 0; i < points.Count; i++)
            {
                uvs[i] = new Vector2((points[i].x - min.x)/ (max.x - min.x),(points[i].z - min.z) / (max.z - min.z));
            }
            currentNode = unusedPoints.First;
            LinkedListNode<Vertex> chekingNode = null;
            LinkedListNode<Vertex> prevVertexNode = unusedPoints.Last;
            LinkedListNode<Vertex> nextVertexNode = currentNode.Next;
            while (unusedPoints.Count > 3)
            {
                prevVertexNode = currentNode.Previous ?? unusedPoints.Last;
                nextVertexNode = currentNode.Next ?? unusedPoints.First;
                if (IsEar(prevVertexNode.Value.position, currentNode.Value.position, nextVertexNode.Value.position))
                {
                    tris[triangleIndex * 3 + 2] = prevVertexNode.Value.index;
                    tris[triangleIndex * 3 + 1] = currentNode.Value.index;
                    tris[triangleIndex * 3] = nextVertexNode.Value.index;
                    triangleIndex++;
                    unusedPoints.Remove(currentNode);
                    currentNode = prevVertexNode;
                    chekingNode = currentNode;
                }
                else
                {
                    if (currentNode.Next == chekingNode) {
                        Debug.LogError("Using a non-simple polygon. Cannot triangulate!");
                        return;
                    };
                    if (currentNode.Next == null) currentNode = unusedPoints.First;
                    else currentNode = currentNode.Next;
                }
            }
            prevVertexNode = currentNode.Previous ?? unusedPoints.Last;
            nextVertexNode = currentNode.Next ?? unusedPoints.First;
            tris[triangleIndex * 3 + 2] = prevVertexNode.Value.index;
            tris[triangleIndex * 3 + 1] = currentNode.Value.index;
            tris[triangleIndex * 3] = nextVertexNode.Value.index;
            triangleIndex++;
            if (twoSided) generateOtherSide();
            setMesh("CustomPolyPrimitive");
        }

        //Custom Vertex for triangulation
        public class Vertex
        {
            public readonly Vector3 position;
            public readonly int index;
            public Vertex(Vector3 position, int index)
            {
                this.position = position;
                this.index = index;
            }
        }

        //Ear test for ear clipping algorithm
        bool IsEar(Vector3 a, Vector3 b, Vector3 c)
        {
            if (!IsConvex(a, c, b)) return false;
            LinkedListNode<Vertex> currentNode = unusedPoints.First;
            for (int i = 0; i < unusedPoints.Count; i++)
            {
                if (currentNode.Value.position != a && currentNode.Value.position != b && currentNode.Value.position != c)
                {
                    if (PointInTriangle(a, b, c, currentNode.Value.position)) return false;
                }
                currentNode = currentNode.Next;
            }
            return true;
        }

        //Convex test for ear clipping algorithm
        public bool IsConvex(Vector3 a, Vector3 b, Vector3 c)
        {
            return (int)Mathf.Sign((c.x - a.x) * (-b.z + a.z) + (c.z - a.z) * (b.x - a.x)) == -1;
        }

        //Point in triangle test for ear clipping algorithm
        public static bool PointInTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
        {
            float area = 0.5f * (-b.z * c.x + a.z * (-b.x + c.x) + a.x * (b.z - c.z) + b.x * c.z);
            float s = 1 / (2 * area) * (a.z * c.x - a.x * c.z + (c.z - a.z) * p.x + (a.x - c.x) * p.z);
            float t = 1 / (2 * area) * (a.x * b.z - a.z * b.x + (a.z - b.z) * p.x + (b.x - a.x) * p.z);
            return s >= 0 && t >= 0 && (s + t) <= 1;
        }

        public override void updateOutline()
        {
            outline = new Outline[points.Count];
            int outlineIndex = 0;
            outlineMaxDistance = 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                outline[outlineIndex] = new Outline(i, i + 1, 0);
                outlineIndex++;
                outlineMaxDistance += (points[i + 1] - points[i]).magnitude;
            }
            outline[outlineIndex] = new Outline(points.Count - 1, 0, 1);
            outline[outlineIndex].closingVertice = 0;
            outlineIndex++;
            outlineMaxDistance += (points[points.Count-1] - points[0]).magnitude;
        }

    }
}