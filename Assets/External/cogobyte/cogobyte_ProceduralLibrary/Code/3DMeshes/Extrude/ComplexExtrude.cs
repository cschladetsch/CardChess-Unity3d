using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    [System.Serializable]
    public class ComplexExtrude : SolidMesh
    {
        public Primitive[] polygonsToExtrude;
        public Matrix4x4[] extrudePath;
        public int[] openCloseExtrude;
        public float uvMidBorder = 0.1f;

        [System.NonSerialized]
        public int vertexNumber;
        [System.NonSerialized]
        public int trisNumber;
        [System.NonSerialized]
        public int vertIndex;
        [System.NonSerialized]
        public int trisIndex;
        [System.NonSerialized]
        public int uvIndex;
        [System.NonSerialized]
        public float maxPathLength;
        [System.NonSerialized]
        public float currentPathLength;

        [System.NonSerialized]
        public Outline[] nextOutline;
        [System.NonSerialized]
        public Outline[] previousOutline;


        public bool useShapeColors;
        public Gradient[] colorFunctions;
        

        //Generates vertices along extrude path that have no cup
        public virtual void generateOutlineVertices(int polyIndex)
        {
            int beforeVertIndex = vertIndex;
            doubleVerts.Add(beforeVertIndex);
            //Take only outline vertices from shape
            float nextOutlineMaxLength = 0;
            for (int i = 0; i < nextOutline.Length; i++)
            {
                nextOutlineMaxLength += (polygonsToExtrude[polyIndex].verts[nextOutline[i].secondIndex] - polygonsToExtrude[polyIndex].verts[nextOutline[i].firstIndex]).magnitude;
                verts[vertIndex] = transformOffset.MultiplyPoint(extrudePath[polyIndex].MultiplyPoint(polygonsToExtrude[polyIndex].verts[nextOutline[i].firstIndex]));
                vertIndex++;
            }
            //aditional vertice for uv
            verts[vertIndex] = transformOffset.MultiplyPoint(extrudePath[polyIndex].MultiplyPoint(polygonsToExtrude[polyIndex].verts[nextOutline[nextOutline.Length - 1].secondIndex]));
            doubleVertsSecond.Add(vertIndex);
            vertIndex++;
            //make uv
            float currentOutlinelength = 0;
            for (int i = 0; i < nextOutline.Length; i++)
            {
                uvs[uvIndex] = new Vector2(currentOutlinelength / nextOutlineMaxLength, uvMidBorder + (1 - 2 * uvMidBorder) * currentPathLength / maxPathLength);
                if (useShapeColors)
                {
                    colors[uvIndex] = polygonsToExtrude[polyIndex].colors[nextOutline[i].firstIndex];
                }
                else
                {
                    colors[uvIndex] = colorFunctions[i / (nextOutline.Length / colorFunctions.Length + 1)].Evaluate(currentPathLength / maxPathLength);
                }
                uvIndex++;
                currentOutlinelength += (polygonsToExtrude[polyIndex].verts[nextOutline[i].secondIndex] - polygonsToExtrude[polyIndex].verts[nextOutline[i].firstIndex]).magnitude;
            }
            uvs[uvIndex] = new Vector2(uvMidBorder, uvMidBorder + (1 - 2 * uvMidBorder) * currentPathLength / maxPathLength);
            colors[uvIndex] = colors[uvIndex - 1];
            uvIndex++;
            //Reconfigure outline that will be used later (to only include outline vertices of new vertice array)
            Outline[] outlineConstruction = new Outline[nextOutline.Length];
            for (int i = 0; i < nextOutline.Length; i++)
            {
               outlineConstruction[i] = new Outline(beforeVertIndex + i, beforeVertIndex + i + 1, nextOutline[i].closingVertice);
            }
            nextOutline = outlineConstruction;
        }

        //Generates cap vertices (for open and closed)
        public virtual void generateShapeVertices(int polyIndex, bool startShape)
        {
            //2D primitive vertices
            for (int i = 0; i < polygonsToExtrude[polyIndex].verts.Length; i++)
            {
                verts[vertIndex] = transformOffset.MultiplyPoint(extrudePath[polyIndex].MultiplyPoint(polygonsToExtrude[polyIndex].verts[i]));
                vertIndex++;
            }
            int temUvIndex = uvIndex;
            if (startShape)
            {
                for (int i = 0; i < polygonsToExtrude[polyIndex].uvs.Length; i++)
                {
                    uvs[uvIndex] = new Vector2(polygonsToExtrude[polyIndex].uvs[i].x, uvMidBorder * polygonsToExtrude[polyIndex].uvs[i].y);
                    colors[uvIndex] = polygonsToExtrude[polyIndex].colors[i];
                    uvIndex++;
                }
            }
            else
            {
                for (int i = 0; i < polygonsToExtrude[polyIndex].uvs.Length; i++)
                {
                    uvs[uvIndex] = new Vector2(polygonsToExtrude[polyIndex].uvs[i].x, (1 - uvMidBorder) + uvMidBorder * polygonsToExtrude[polyIndex].uvs[i].y);
                    colors[uvIndex] = polygonsToExtrude[polyIndex].colors[i];
                    uvIndex++;
                }
            }
            if (!useShapeColors)
            {
                for (int i = 0; i < polygonsToExtrude[polyIndex].outline.Length; i++)
                {
                    colors[temUvIndex + polygonsToExtrude[polyIndex].outline[i].firstIndex] = colorFunctions[i / (nextOutline.Length / colorFunctions.Length + 1)].Evaluate(currentPathLength / maxPathLength);
                }
            }
        }

        //Generates triangles along extrude path that have no cup
        private void generateOutlineTriangles(int polyIndex)
        {
            if (polyIndex != 0 && openCloseExtrude[polyIndex] != 0)
            {
                if (polygonsToExtrude[polyIndex] == polygonsToExtrude[polyIndex - 1])
                {
                    //normal extrude
                    for (int i = 0; i < nextOutline.Length; i++)
                    {
                        //firsttris
                        tris[trisIndex] = nextOutline[i].firstIndex;
                        trisIndex++;
                        tris[trisIndex] = previousOutline[i].secondIndex;
                        trisIndex++;
                        tris[trisIndex] = previousOutline[i].firstIndex;
                        trisIndex++;
                        //secondtris
                        tris[trisIndex] = nextOutline[i].firstIndex;
                        trisIndex++;
                        tris[trisIndex] = nextOutline[i].secondIndex;
                        trisIndex++;
                        tris[trisIndex] = previousOutline[i].secondIndex;
                        trisIndex++;
                    }
                }
                else
                {
                    //extrude different shapes
                    Vector3 v = new Vector3(1, 0, 0);
                    //they start with 0 by default, you need to inherit class and change findStartIndex for different behavior
                    int previousDistIndex = findStartIndex(previousOutline, v);
                    int nextDistIndex = findStartIndex(nextOutline, v);
                    int prevVertCounter = previousDistIndex;
                    int nextVertCounter = nextDistIndex;
                    bool first = false;
                    bool second = false;
                    float lengthPrevious = 0;
                    float lengthNext = 0;
                    if (previousOutline[0].firstIndex == previousOutline[0].secondIndex) first = true;
                    if (nextOutline[0].firstIndex == nextOutline[0].secondIndex) second = true;
                    for (int i = 0; i < previousOutline.Length; i++)
                    {
                        lengthPrevious += (verts[previousOutline[i].secondIndex] - verts[previousOutline[i].firstIndex]).magnitude;
                    }
                    for (int i = 0; i < nextOutline.Length; i++)
                    {
                        lengthNext += (verts[nextOutline[i].secondIndex] - verts[nextOutline[i].firstIndex]).magnitude;
                    }
                    float currentLengthPrevious = 0;
                    float currentLengthNext = 0;
                    float a = 0;
                    float b = 0;
                    while (!first || !second)
                    {
                        if (!first)
                        {
                            a = currentLengthPrevious + (verts[previousOutline[prevVertCounter].firstIndex] - verts[previousOutline[prevVertCounter].secondIndex]).magnitude;
                        }
                        if (!second)
                        {
                            b = currentLengthNext + (verts[nextOutline[nextVertCounter].firstIndex] - verts[nextOutline[nextVertCounter].secondIndex]).magnitude;
                        }

                        if (second || (!first && Mathf.Abs(a / lengthPrevious - currentLengthNext / lengthNext) < Mathf.Abs(b / lengthNext - currentLengthPrevious / lengthPrevious)))
                        {
                            tris[trisIndex] = previousOutline[prevVertCounter].secondIndex;
                            trisIndex++;
                            tris[trisIndex] = previousOutline[prevVertCounter].firstIndex;
                            trisIndex++;
                            tris[trisIndex] = nextOutline[nextVertCounter].firstIndex;
                            trisIndex++;
                            if (prevVertCounter + 1 >= previousOutline.Length)
                            {
                                prevVertCounter = 0;
                            }
                            else
                            {
                                prevVertCounter++;
                            }
                            if (prevVertCounter == previousDistIndex) first = true;
                            currentLengthPrevious = a;
                        }
                        else
                        {
                            tris[trisIndex] = nextOutline[nextVertCounter].secondIndex;
                            trisIndex++;
                            tris[trisIndex] = previousOutline[prevVertCounter].firstIndex;
                            trisIndex++;
                            tris[trisIndex] = nextOutline[nextVertCounter].firstIndex;
                            trisIndex++;
                            if (nextVertCounter + 1 >= nextOutline.Length)
                            {
                                nextVertCounter = 0;
                            }
                            else
                            {
                                nextVertCounter++;
                            }
                            if (nextVertCounter == nextDistIndex) second = true;
                            currentLengthNext = b;
                        }
                    }
                }
            }
        }

        //generates triangles for begin cap
        private void generateBeginShapeTriangles(int polyIndex, int beforeVertIndex)
        {
            //need to swap vertices for inverted triangles
            for (int i = 0; i < polygonsToExtrude[polyIndex].tris.Length; i++)
            {
                tris[trisIndex] = beforeVertIndex + polygonsToExtrude[polyIndex].tris[i];
                trisIndex++;
                i++;
                tris[trisIndex] = beforeVertIndex + polygonsToExtrude[polyIndex].tris[i + 1];
                trisIndex++;
                i++;
                tris[trisIndex] = beforeVertIndex + polygonsToExtrude[polyIndex].tris[i - 1];
                trisIndex++;
            }
        }

        //generates triangles for end cap
        private void generateEndShapeTriangles(int polyIndex, int beforeVertIndex)
        {
            //just copy the triangles + offset
            for (int i = 0; i < polygonsToExtrude[polyIndex].tris.Length; i++)
            {
                tris[trisIndex] = polygonsToExtrude[polyIndex].tris[i] + beforeVertIndex;
                trisIndex++;
            }
        }

        public void extrudeOnePart(int polyIndex)
        {
            nextOutline = polygonsToExtrude[polyIndex].getOutline();
            int beforeVertIndex = vertIndex;
            if (openCloseExtrude[polyIndex] == 0)
            {
                generateShapeVertices(polyIndex, true);
                generateBeginShapeTriangles(polyIndex, beforeVertIndex);
            }
            generateOutlineVertices(polyIndex);
            if (openCloseExtrude[polyIndex] != 0)
            {
                generateOutlineTriangles(polyIndex);
            }
            beforeVertIndex = vertIndex;
            if (openCloseExtrude[polyIndex] == 2)
            {
                generateShapeVertices(polyIndex, false);
                generateEndShapeTriangles(polyIndex, beforeVertIndex);
            }
            previousOutline = nextOutline;
        }

        public int findStartIndex(Outline[] outline, Vector3 angleVector)
        {
            return 0;
        }

        public override int getVertexCount()
        {
            return vertexNumber;
        }

        public override int getTrisCount()
        {
            return trisNumber;
        }

        public override void generate()
        {
            doubleVerts = new List<int>();
            doubleVertsSecond = new List<int>();
            vertIndex = 0;
            trisIndex = 0;
            uvIndex = 0;
            vertexNumber = 0;
            trisNumber = 0;
            int lastVertNum = 0;
            int currentVertNum = 0;
            maxPathLength = 0;
            currentPathLength = 0;
            Vector3 fixedZeroVector = new Vector3(0, 0, 0);
            for (int i = 0; i < polygonsToExtrude.Length; i++)
            {
                if (i < polygonsToExtrude.Length - 1)
                {
                    maxPathLength += (extrudePath[i + 1].MultiplyPoint(fixedZeroVector) - extrudePath[i].MultiplyPoint(fixedZeroVector)).magnitude;
                }
                currentVertNum = polygonsToExtrude[i].getOutline().Length + 1;
                if (openCloseExtrude[i] != 1)
                {
                    vertexNumber += polygonsToExtrude[i].verts.Length;
                    trisNumber += polygonsToExtrude[i].tris.Length;
                }
                vertexNumber += currentVertNum;
                if (i != 0 && openCloseExtrude[i] != 0)
                {
                    trisNumber += (lastVertNum + currentVertNum) * 3;
                }
                lastVertNum = currentVertNum;
            }
            base.generate();
            prepareColorGradient(ref colorFunctions);
            extrudeOnePart(0);
            for (int i = 1; i < polygonsToExtrude.Length; i++)
            {
                extrudeOnePart(i);
                currentPathLength += (extrudePath[i].MultiplyPoint(fixedZeroVector) - extrudePath[i - 1].MultiplyPoint(fixedZeroVector)).magnitude;
            }
            setMesh("ComplexExtrude");
            FixNormals();
            mesh.normals = normals;
        }
    }
}