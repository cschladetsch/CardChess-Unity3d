using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Custom mesh transfers the complete mesh to a cogobyte_Pl_3DMesh. Used mesh should have vertex colors
    [CreateAssetMenu(fileName = "CustomMesh", menuName = "Cogobyte/ProceduralLibrary/3DMeshes/CustomMesh", order = 2)]
    public class CustomMesh : SolidMesh
    {
        public Mesh customMesh;

        public override int getVertexCount()
        {
            return customMesh.vertices.Length;
        }

        public override int getTrisCount()
        {
            return customMesh.triangles.Length;
        }

        public override void generate()
        {
            base.generate();
            Vector3[] tempVertices = customMesh.vertices;
            for (int i = 0; i < tempVertices.Length; i++)
            {
                verts[i] = transformOffset.MultiplyPoint(tempVertices[i]);
            }
            uvs = customMesh.uv;
            tris = customMesh.triangles;
            if (customMesh.colors32 == null || customMesh.colors32.Length != verts.Length)
            {
                colors = new Color32[verts.Length];
                for(int i = 0; i < verts.Length; i++)
                {
                    colors[i] = Color.white;
                }
            }
            else
            {
                colors = customMesh.colors32;
            }
            normals = customMesh.normals;
            setMesh("CustomMesh");
        }
    }
}
