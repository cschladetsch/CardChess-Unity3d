using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    [System.Serializable]
    public class FlatShadeExtrude : ComplexExtrude
    {
        public override void generateOutlineVertices(int polyIndex)
        {
            int beforeVertIndex = vertIndex;
            //Take only outline vertices from shape
            float nextOutlineMaxLength = 0;
            Matrix4x4 trans = new Matrix4x4();
            trans.SetTRS(extrudePath[polyIndex].GetColumn(3), Quaternion.LookRotation(extrudePath[polyIndex].GetColumn(2), extrudePath[polyIndex].GetColumn(1)), new Vector3(1,1,1));

            for (int i = 0; i < nextOutline.Length; i++)
            {
                nextOutlineMaxLength += (polygonsToExtrude[polyIndex].verts[nextOutline[i].secondIndex] - polygonsToExtrude[polyIndex].verts[nextOutline[i].firstIndex]).magnitude;
                verts[vertIndex] = transformOffset.MultiplyPoint(extrudePath[polyIndex].MultiplyPoint(polygonsToExtrude[polyIndex].verts[nextOutline[i].firstIndex]));
                vertIndex++;
                verts[vertIndex] = transformOffset.MultiplyPoint(extrudePath[polyIndex].MultiplyPoint(polygonsToExtrude[polyIndex].verts[nextOutline[i].secondIndex]));
                normals[vertIndex] = -Vector3.Cross(transformOffset.MultiplyPoint(trans.MultiplyPoint(polygonsToExtrude[polyIndex].verts[nextOutline[i].secondIndex])) - transformOffset.MultiplyPoint(trans.MultiplyPoint(polygonsToExtrude[polyIndex].verts[nextOutline[i].firstIndex])), transformOffset.MultiplyVector(trans.MultiplyVector(Vector3.up))).normalized;//-Vector3.Cross(transformOffset.MultiplyPoint(trans.MultiplyPoint(polygonsToExtrude[polyIndex].verts[nextOutline[i].secondIndex] - polygonsToExtrude[polyIndex].verts[nextOutline[i].firstIndex])), transformOffset.MultiplyVector(extrudePath[polyIndex].MultiplyVector(Vector3.up))).normalized;
                normals[vertIndex-1] = normals[vertIndex];
                vertIndex++;
            }
            //make uv
            for (int i = 0; i < nextOutline.Length; i++)
            {
                uvs[uvIndex] = new Vector2(0, uvMidBorder + (1 - 2 * uvMidBorder) * currentPathLength / maxPathLength);
                if (useShapeColors)
                {
                    colors[uvIndex] = polygonsToExtrude[polyIndex].colors[nextOutline[i].firstIndex];
                }
                else
                {
                    colors[uvIndex] = colorFunctions[i / (nextOutline.Length / colorFunctions.Length + 1)].Evaluate(currentPathLength / maxPathLength);
                }
                uvIndex++;
                uvs[uvIndex] = new Vector2(1, uvMidBorder + (1 - 2 * uvMidBorder) * currentPathLength / maxPathLength);
                if (useShapeColors)
                {
                    colors[uvIndex] = polygonsToExtrude[polyIndex].colors[nextOutline[i].firstIndex];
                }
                else
                {
                    colors[uvIndex] = colorFunctions[i / (nextOutline.Length / colorFunctions.Length + 1)].Evaluate(currentPathLength / maxPathLength);
                }
                uvIndex++;
            }
            //Reconfigure outline that will be used later (to only include outline vertices of new vertice array)
            Outline[] outlineConstruction = new Outline[nextOutline.Length];
            for (int i = 0; i < nextOutline.Length; i++)
            {
                outlineConstruction[i] = new Outline(beforeVertIndex + i * 2, beforeVertIndex + i * 2 + 1, nextOutline[i].closingVertice);
            }
            nextOutline = outlineConstruction;
        }

        public override void generateShapeVertices(int polyIndex, bool startShape)
        {
            int beginVertIndex = vertIndex;
            int vertexIndexCounter = beginVertIndex;
            base.generateShapeVertices(polyIndex, startShape);
            for (int i = 0; i < polygonsToExtrude[polyIndex].verts.Length; i++)
            {
                if (startShape) { normals[vertexIndexCounter] = transformOffset.MultiplyVector(extrudePath[polyIndex].MultiplyVector(Vector3.down)).normalized; }
                else { normals[vertexIndexCounter] = transformOffset.MultiplyVector(extrudePath[polyIndex].MultiplyVector(Vector3.up)).normalized; }
                vertexIndexCounter++;
            }
            Matrix4x4 trans = extrudePath[polyIndex].transpose;
            trans = trans.transpose;
            trans.m00 = 1;
            trans.m11 = 1;
            trans.m22 = 1;
            for (int i = 0; i < polygonsToExtrude[polyIndex].outline.Length; i++)
            {

                normals[beginVertIndex + polygonsToExtrude[polyIndex].outline[i].firstIndex] = (normals[beginVertIndex + polygonsToExtrude[polyIndex].outline[i].firstIndex] - Vector3.Cross(transformOffset.MultiplyVector(trans.MultiplyVector(polygonsToExtrude[polyIndex].verts[polygonsToExtrude[polyIndex].outline[i].secondIndex] - polygonsToExtrude[polyIndex].verts[polygonsToExtrude[polyIndex].outline[i].firstIndex])), transformOffset.MultiplyVector(extrudePath[polyIndex].MultiplyVector(Vector3.up)))).normalized;
            }
        }

        public override void generate()
        {
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
                currentVertNum = polygonsToExtrude[i].getOutline().Length * 2;
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
            prepareOffset();
            prepareMesh();
            normals = new Vector3[getVertexCount()];
            prepareColorGradient(ref colorFunctions);
            extrudeOnePart(0);
            for (int i = 1; i < polygonsToExtrude.Length; i++)
            {
                extrudeOnePart(i);
                currentPathLength += (extrudePath[i].MultiplyPoint(fixedZeroVector) - extrudePath[i - 1].MultiplyPoint(fixedZeroVector)).magnitude;
            }
            setMesh("ComplexExtrude");
            mesh.normals = normals;            
        }

    }
}