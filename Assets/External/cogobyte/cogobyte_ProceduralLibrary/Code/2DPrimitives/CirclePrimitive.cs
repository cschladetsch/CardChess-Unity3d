using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Circle shape
    [CreateAssetMenu(fileName = "CirclePrimitive", menuName = "Cogobyte/ProceduralLibrary/Primitives/CirclePrimitive", order = 2)]
    [System.Serializable]
    public class CirclePrimitive : Primitive
    {
        //radius
        public float radius = 1;
        //Level of detail by range and by section (vertices are generated in circles detailed by segments from center to maximum range)
        [Range(3, 80)]
        public int numberOfSectors = 3;
        [Range(1, 80)]
        public int numberOfSectorSections = 1;
        
        //Coloring for each circle segment from center to maximum range
        public Gradient[] gradient;

        void OnValidate()
        {
            radius = Mathf.Max(radius, 0);
            prepareColorGradient(ref gradient);
        }

        public override int getVertexCount()
        {
            if (twoSided) return (numberOfSectors * numberOfSectorSections + 1) * 2;
            return numberOfSectors * numberOfSectorSections + 1;
        }

        public override int getTrisCount()
        {
            if (twoSided) return (numberOfSectors * 3 + (numberOfSectors * 2 * (numberOfSectorSections - 1)) * 3) * 2;
            return numberOfSectors * 3 + (numberOfSectors * 2 * (numberOfSectorSections - 1)) * 3;
        }

        public override void generate()
        {
            base.generate();
            prepareColorGradient(ref gradient);
            normals = new Vector3[getVertexCount()];
            int uvIndex = 0;
            int vertexIndex = 0;
            float angle = -Mathf.PI * 1 / 2;
            float maxAngle = 2 * Mathf.PI;
            verts[0] = transformOffset.MultiplyPoint(new Vector3(0, 0, 0));
            normals[0] = transformOffset.MultiplyVector(Vector3.up);
            colors[0] = gradient[0].Evaluate(0);
            vertexIndex++;
            uvs[uvIndex] = new Vector2(0.5f, 0.5f);
            uvIndex++;
            while (vertexIndex < numberOfSectors + 1)
            {
                for (int i = 0; i < numberOfSectorSections; i++)
                {
                    verts[vertexIndex + i * numberOfSectors] = transformOffset.MultiplyPoint(new Vector3((i + 1) * radius / numberOfSectorSections * Mathf.Cos(angle), 0, (i + 1) * radius / numberOfSectorSections * Mathf.Sin(angle)));
                    uvs[vertexIndex + i * numberOfSectors] = new Vector2(0.5f + (i + 1) * 1f / numberOfSectorSections * 0.5f * Mathf.Cos(angle), 0.5f + (i + 1) * 1f / numberOfSectorSections * 0.5f * Mathf.Sin(angle));
                    normals[vertexIndex + i * numberOfSectors] = transformOffset.MultiplyVector(Vector3.up);
                    colors[vertexIndex + i * numberOfSectors] = gradient[i % gradient.Length].Evaluate((((float)vertexIndex - 1) / numberOfSectors));
                }
                angle += maxAngle / numberOfSectors;
                uvIndex++;
                vertexIndex++;
            }
            int triangleIndex = 0;
            for (int i = 0; i < numberOfSectors - 1; i++)
            {
                tris[triangleIndex] = 0;
                triangleIndex++;
                tris[triangleIndex] = i + 2;
                triangleIndex++;
                tris[triangleIndex] = i + 1;
                triangleIndex++;
            }
            tris[triangleIndex] = 0;
            triangleIndex++;
            tris[triangleIndex] = 1;
            triangleIndex++;
            tris[triangleIndex] = numberOfSectors;
            triangleIndex++;
            for (int k = 0; k < numberOfSectorSections - 1; k++)
            {
                for (int i = 0; i < numberOfSectors - 1; i++)
                {
                    tris[triangleIndex] = (k + 1) * numberOfSectors + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * numberOfSectors + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * numberOfSectors + i + 2;
                    triangleIndex++;
                    //second
                    tris[triangleIndex] = (k + 1) * numberOfSectors + i + 2;
                    triangleIndex++;
                    tris[triangleIndex] = (k + 1) * numberOfSectors + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * numberOfSectors + i + 2;
                    triangleIndex++;
                }
                tris[triangleIndex] = (k + 1) * numberOfSectors + numberOfSectors;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSectors + numberOfSectors;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSectors + 1;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSectors + numberOfSectors + 1;
                triangleIndex++;
                tris[triangleIndex] = (k + 1) * numberOfSectors + numberOfSectors;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSectors + 1;
                triangleIndex++;
            }
            if (twoSided) generateOtherSide();
            setMesh("2DPrimitiveCircle");
        }

        public override void updateOutline()
        {
            outline = new Outline[numberOfSectors];
            int outlineIndex = 0;
            outlineMaxDistance = 0;
            int vertexCount = getVertexCount();
            for (int i = numberOfSectors; i >= 1; i--)
            {
                outline[outlineIndex] = new Outline(vertexCount - i, vertexCount - i + 1, 0);
                outlineIndex++;
            }
            outline[outlineIndex - 1].secondIndex = vertexCount - numberOfSectors;
            outline[outlineIndex - 1].closingVertice = vertexCount - numberOfSectors;
            Vector3[] tempVerts = mesh.vertices;
            for (int i = 0; i < outlineIndex; i++)
            {
                outlineMaxDistance += (tempVerts[outline[i].secondIndex] - tempVerts[outline[i].firstIndex]).magnitude;
            }
        }
    }
}