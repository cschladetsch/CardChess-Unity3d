using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //3D Sphere mesh
    [CreateAssetMenu(fileName = "Sphere", menuName = "Cogobyte/ProceduralLibrary/3DMeshes/Sphere", order = 3)]
    public class SphereMesh : SolidMesh
    {
        public float radius = 1;
        //Level of detail
        [Range(1, 80)]
        public int numberOfSectionsParallel = 3;
        [Range(3, 80)]
        public int numberOfSectionsMeridian = 7;
        //Colloring
        public Gradient[] gradient;

        void OnValidate()
        {
            radius = Mathf.Max(radius, 0);
            prepareColorGradient(ref gradient);
        }

        public override int getVertexCount()
        {
            return (numberOfSectionsMeridian + 1) * ((numberOfSectionsParallel) * 2 + 1);
        }

        public override int getTrisCount()
        {
            return (numberOfSectionsParallel * 2) * numberOfSectionsMeridian * 6;
        }

        public override void generate()
        {
            base.generate();
            prepareColorGradient(ref gradient);
            int vertexIndex = 0;
            float angleFI;
            float angleTheta;
            for (int i = 0; i < numberOfSectionsParallel; i++)
            {
                for (int j = 0; j <= numberOfSectionsMeridian; j++)
                {
                    angleFI = -Mathf.PI + i * Mathf.PI / (numberOfSectionsParallel * 2);
                    angleTheta = j * 2 * Mathf.PI / numberOfSectionsMeridian;
                    verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(radius * Mathf.Cos(angleTheta) * Mathf.Sin(angleFI), radius * Mathf.Cos(angleFI), radius * Mathf.Sin(angleTheta) * Mathf.Sin(angleFI)));
                    normals[vertexIndex] = verts[vertexIndex] - transformOffset.MultiplyPoint(new Vector3(0, 0, 0));
                    uvs[vertexIndex] = new Vector2(j * 1f / numberOfSectionsMeridian, i * 0.5f / numberOfSectionsParallel);
                    colors[vertexIndex] = gradient[i % gradient.Length].Evaluate(j / numberOfSectionsMeridian);
                    vertexIndex++;
                }
            }
            for (int j = 0; j <= numberOfSectionsMeridian; j++)
            {
                angleTheta = j * 2 * Mathf.PI / numberOfSectionsMeridian;
                verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(-radius * Mathf.Cos(angleTheta), 0, -radius * Mathf.Sin(angleTheta)));
                normals[vertexIndex] = verts[vertexIndex] - transformOffset.MultiplyPoint(new Vector3(0, 0, 0));
                uvs[vertexIndex] = new Vector2(j * 1f / numberOfSectionsMeridian, 0.5f);
                colors[vertexIndex] = gradient[numberOfSectionsParallel % gradient.Length].Evaluate(j / numberOfSectionsMeridian);
                vertexIndex++;
            }
            for (int i = 0; i < numberOfSectionsParallel; i++)
            {
                for (int j = 0; j <= numberOfSectionsMeridian; j++)
                {
                    angleFI = -Mathf.PI / 2 + (i + 1) * Mathf.PI / (numberOfSectionsParallel * 2);
                    angleTheta = j * 2 * Mathf.PI / numberOfSectionsMeridian;
                    verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(radius * Mathf.Cos(angleTheta) * Mathf.Sin(angleFI), radius * Mathf.Cos(angleFI), radius * Mathf.Sin(angleTheta) * Mathf.Sin(angleFI)));
                    normals[vertexIndex] = verts[vertexIndex] - transformOffset.MultiplyPoint(new Vector3(0, 0, 0));
                    uvs[vertexIndex] = new Vector2(j * 1f / numberOfSectionsMeridian, 0.5f + (i + 1) * 0.5f / numberOfSectionsParallel);
                    colors[vertexIndex] = gradient[(numberOfSectionsParallel + 1 + i) % gradient.Length].Evaluate(j / numberOfSectionsMeridian);
                    vertexIndex++;
                }
            }
            int triangleIndex = 0;
            for (int k = 0; k < numberOfSectionsParallel * 2; k++)
            {
                for (int i = 0; i < numberOfSectionsMeridian; i++)
                {
                    tris[triangleIndex] = (k + 1) * (numberOfSectionsMeridian + 1) + i;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * (numberOfSectionsMeridian + 1) + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * (numberOfSectionsMeridian + 1) + i;
                    triangleIndex++;
                    //second
                    tris[triangleIndex] = (k + 1) * (numberOfSectionsMeridian + 1) + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * (numberOfSectionsMeridian + 1) + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k + 1) * (numberOfSectionsMeridian + 1) + i;
                    triangleIndex++;
                }
            }
            setMesh("ProceduralSphere");
        }
    }
}
