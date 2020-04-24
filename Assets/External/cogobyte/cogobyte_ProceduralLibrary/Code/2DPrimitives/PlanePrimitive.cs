using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Square Plane primitive shape
    [CreateAssetMenu(fileName = "PlanePrimitive", menuName = "Cogobyte/ProceduralLibrary/Primitives/PlanePrimitive", order = 3)]
    [System.Serializable]
    public class PlanePrimitive : Primitive
    {
        public float width = 1;
        public float length = 1;
        [Range(1, 100)]
        public int numberOfSectionsByWidth = 1;
        [Range(1, 100)]
        public int numberOfSectionsByLength = 1;
        public Gradient[] gradient;

        void OnValidate()
        {
            width = Mathf.Max(width, 0);
            length = Mathf.Max(length, 0);
            prepareColorGradient(ref gradient);
        }

        public override int getVertexCount()
        {
            if (twoSided) return (numberOfSectionsByLength + 1) * (numberOfSectionsByWidth + 1) * 2;
            return (numberOfSectionsByLength + 1) * (numberOfSectionsByWidth + 1);
        }

        public override int getTrisCount()
        {
            if (twoSided) return (numberOfSectionsByWidth * 2) * numberOfSectionsByLength * 6;
            return (numberOfSectionsByWidth * 2) * numberOfSectionsByLength * 3;
        }



        public override void generate()
        {
            base.generate();
            prepareColorGradient(ref gradient);
            normals = new Vector3[getVertexCount()];
            int vertexIndex = 0;
            int uvIndex = 0;
            float sectionLength = length / numberOfSectionsByLength;
            float sectionWidth = width / numberOfSectionsByWidth;
            Vector3 displacment = new Vector3(-width / 2, 0, -length / 2);
            for (int i = 0; i <= numberOfSectionsByLength; i++)
            {
                for (int j = 0; j <= numberOfSectionsByWidth; j++)
                {
                    verts[vertexIndex] = new Vector3(j * sectionWidth, 0, i * sectionLength) + displacment;
                    uvs[uvIndex++] = new Vector2(i / (float)numberOfSectionsByLength, j / (float)numberOfSectionsByWidth);
                    normals[vertexIndex] = transformOffset.MultiplyVector(Vector3.up);
                    colors[vertexIndex] = gradient[i % gradient.Length].Evaluate((((float)j) / numberOfSectionsByWidth));
                    vertexIndex++;
                }
            }
            int triangleIndex = 0;
            for (int i = 0; i < numberOfSectionsByLength; i++)
            {
                for (int j = 0; j < numberOfSectionsByWidth; j++)
                {
                    //first triangle
                    tris[triangleIndex] = (i + 1) * (numberOfSectionsByWidth + 1) + j;
                    triangleIndex++;
                    tris[triangleIndex] = (i) * (numberOfSectionsByWidth + 1) + j + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (i) * (numberOfSectionsByWidth + 1) + j;
                    triangleIndex++;
                    //second triangle
                    tris[triangleIndex] = (i + 1) * (numberOfSectionsByWidth + 1) + j;
                    triangleIndex++;
                    tris[triangleIndex] = (i + 1) * (numberOfSectionsByWidth + 1) + j + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (i) * (numberOfSectionsByWidth + 1) + j + 1;
                    triangleIndex++;
                }
            }
            if (twoSided) generateOtherSide();
            setMesh("2DPrimitivePlane");
        }

        public override void updateOutline()
        {
            outline = new Outline[2 * (numberOfSectionsByLength + 1) + 2 * (numberOfSectionsByWidth - 1)];
            int outlineIndex = 0;
            for (int i = numberOfSectionsByWidth / 2; i <= numberOfSectionsByWidth; i++)
            {
                outline[outlineIndex] = new Outline(i, i + 1, 0);
                outlineIndex++;
            }
            outline[outlineIndex - 1].secondIndex = (numberOfSectionsByWidth + 1) + numberOfSectionsByWidth;
            for (int i = 1; i < numberOfSectionsByLength; i++)
            {
                outline[outlineIndex] = new Outline((i + 1) * (numberOfSectionsByWidth + 1) - 1, (i + 2) * (numberOfSectionsByWidth + 1) - 1, 0);
                outlineIndex++;
            }
            outline[outlineIndex - 1].secondIndex = numberOfSectionsByLength * (numberOfSectionsByWidth + 1) + numberOfSectionsByWidth;
            for (int i = numberOfSectionsByWidth; i >= 0; i--)
            {
                int k = (numberOfSectionsByLength) * (numberOfSectionsByWidth + 1);
                outline[outlineIndex] = new Outline(k + i, k + i - 1, 0);
                outlineIndex++;
            }
            outline[outlineIndex - 1].secondIndex = (numberOfSectionsByLength - 1) * (numberOfSectionsByWidth + 1);
            for (int i = numberOfSectionsByLength - 1; i > 0; i--)
            {
                int k = (numberOfSectionsByWidth + 1);
                outline[outlineIndex] = new Outline(k * i, k * (i - 1), 0);
                outlineIndex++;
            }
            outline[outlineIndex - 1].secondIndex = 0;
            outline[outlineIndex - 1].closingVertice = 1;
            for (int i = 0; i < numberOfSectionsByWidth / 2; i++)
            {
                outline[outlineIndex] = new Outline(i, i + 1, 0);
                outlineIndex++;
            }
        }


    }
}