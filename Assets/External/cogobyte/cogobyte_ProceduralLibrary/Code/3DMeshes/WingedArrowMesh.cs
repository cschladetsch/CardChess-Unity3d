using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //A winged arrow mesh Defined by two points left and right, one point for start point of arrow and one for end.
    //Each point can be made of two points with different distances but same x,y positions
    [CreateAssetMenu(fileName = "WingedArrowMesh", menuName = "Cogobyte/ProceduralLibrary/3DMeshes/WingedArrowMesh", order = 4)]
    public class WingedArrowMesh : SolidMesh
    {
        public float length = 1;
        public float leftLength = 1;
        public float rightLength = 1;
        public float leftWingWidth = 0;
        public float rightWingWidth = 0;
        public float startWidth = 1;
        public float endWidth = 0;
        public float leftBackDistance = 1;
        public float rightBackDistance = 1;

        public Color32 leftColor = Color.white;
        public Color32 color = Color.white;
        public Color32 rightColor = Color.white;

        void OnValidate()
        {
            length = Mathf.Max(length, 0);
            leftLength = Mathf.Max(leftLength, 0);
            rightLength = Mathf.Max(rightLength, 0);
            leftWingWidth = Mathf.Max(leftWingWidth, 0);
            rightWingWidth = Mathf.Max(rightWingWidth, 0);
            startWidth = Mathf.Max(startWidth, 0);
            endWidth = Mathf.Max(endWidth, 0);
            leftBackDistance = Mathf.Max(leftBackDistance, 0);
            rightBackDistance = Mathf.Max(rightBackDistance, 0);
        }

        public override int getVertexCount()
        {
            return 8;
        }

        public override int getTrisCount()
        {
            return 12 * 3;
        }

        public override void generate()
        {
            base.generate();
            int vertexIndex = 0;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(0, 0, -startWidth / 2));
            uvs[vertexIndex] = new Vector2(0.5f, 0.4f);
            colors[vertexIndex] = color;
            vertexIndex++;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(0, 0, startWidth / 2));
            uvs[vertexIndex] = new Vector2(0.5f, 0.6f);
            colors[vertexIndex] = color;
            vertexIndex++;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(0, length, -endWidth / 2));
            uvs[vertexIndex] = new Vector2(0.5f, 0f);
            colors[vertexIndex] = color;
            vertexIndex++;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(0, length, endWidth / 2));
            uvs[vertexIndex] = new Vector2(0.5f, 1f);
            colors[vertexIndex] = color;
            vertexIndex++;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(-leftLength, -leftBackDistance, -leftWingWidth / 2));
            uvs[vertexIndex] = new Vector2(0f, 0.4f);
            colors[vertexIndex] = leftColor;
            vertexIndex++;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(-leftLength, -leftBackDistance, leftWingWidth / 2));
            uvs[vertexIndex] = new Vector2(0f, 0.6f);
            colors[vertexIndex] = leftColor;
            vertexIndex++;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(rightLength, -rightBackDistance, -rightWingWidth / 2));
            uvs[vertexIndex] = new Vector2(0f, 0.4f);
            colors[vertexIndex] = rightColor;
            vertexIndex++;
            verts[vertexIndex] = transformOffset.MultiplyPoint(new Vector3(rightLength, -rightBackDistance, rightWingWidth / 2));
            uvs[vertexIndex] = new Vector2(1f, 0.6f);
            colors[vertexIndex] = rightColor;
            vertexIndex++;
            tris[0] = 3;
            tris[1] = 5;
            tris[2] = 1;

            tris[3] = 3;
            tris[4] = 1;
            tris[5] = 7;

            tris[6] = 2;
            tris[7] = 0;
            tris[8] = 4;

            tris[9] = 2;
            tris[10] = 6;
            tris[11] = 0;

            tris[12] = 1;
            tris[13] = 5;
            tris[14] = 0;

            tris[15] = 5;
            tris[16] = 4;
            tris[17] = 0;

            tris[18] = 0;
            tris[19] = 7;
            tris[20] = 1;

            tris[21] = 7;
            tris[22] = 0;
            tris[23] = 6;

            tris[24] = 7;
            tris[25] = 2;
            tris[26] = 3;

            tris[27] = 7;
            tris[28] = 6;
            tris[29] = 2;

            tris[30] = 2;
            tris[31] = 5;
            tris[32] = 3;

            tris[33] = 2;
            tris[34] = 4;
            tris[35] = 5;

            setMesh("ProceduralWingedArrowMesh");
            FixNormals();
        }
    }
}
