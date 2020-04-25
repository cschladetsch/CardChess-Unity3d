using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Abstract class for 2D shapes
    [System.Serializable]
    public abstract class Primitive : ProceduralMesh
    {
        public Outline[] outline;
        [System.NonSerialized]
        public float outlineMaxDistance;
        //Generate triangles on other side
        public bool twoSided = false;

        public Outline[] getOutline()
        {
            if (outline == null)
            {
                updateOutline();
            }
            return outline;
        }

        public virtual void updateOutline()
        {
        }

        //Doubles the triangles to make other side

        public void generateOtherSide()
        {
            int vertIndex = getVertexCount() / 2;
            for(int i = 0; i < vertIndex; i++)
            {
                verts[vertIndex + i] = verts[i];
                uvs[vertIndex + i] = uvs[i];
                normals[vertIndex + i] = transformOffset.MultiplyVector(Vector3.down);
                colors[vertIndex + i] = colors[i];
            }
            int trisIndex = getTrisCount() / 2;
            for(int i=0;i< trisIndex; i+=3)
            {
                tris[trisIndex + i] = tris[i] + vertIndex;
                tris[trisIndex +i+ 1] = tris[i+2] + vertIndex;
                tris[trisIndex +i+ 2] = tris[i+1] + vertIndex;
            }
        }

    }
}