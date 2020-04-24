using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cogobyte.ProceduralIndicators
{
    //Editor for the mesh grid indicator
    [CustomEditor(typeof(MeshGridIndicator))]
    [CanEditMultipleObjects]
    public class MeshGridIndicatorEditor : Editor {
        public MeshGridIndicator gridIndicator;

        void OnEnable()
        {
            gridIndicator = target as MeshGridIndicator;
        }

        //Draws a wire mesh for each cell of the grid
        public virtual void OnSceneGUI()
        {
            if (gridIndicator.mesh == null) return;
            if (gridIndicator.gridX.Length == 0) { gridIndicator.gridX = new float[1] { 1 }; }
            if (gridIndicator.gridY.Length == 0) { gridIndicator.gridY = new float[1] { 1 }; }
            if (gridIndicator.gridZ.Length == 0) { gridIndicator.gridZ = new float[1] { 1 }; }
            Handles.color = gridIndicator.defaultColor;
            float distanceX = 0;
            float distanceY = 0;
            float distanceZ = 0;
            for (int i = 0; i < gridIndicator.gridX.Length; i++)
            {
                for (int j = 0; j < gridIndicator.gridY.Length; j++)
                {
                    for (int k = 0; k < gridIndicator.gridZ.Length; k++)
                    {
                        Handles.DrawWireCube(gridIndicator.gridOffset + gridIndicator.transform.position + new Vector3(distanceX, distanceY, distanceZ) + new Vector3(gridIndicator.gridX[i], gridIndicator.gridY[j], gridIndicator.gridZ[k])/2, new Vector3(gridIndicator.gridX[i], gridIndicator.gridY[j], gridIndicator.gridZ[k]));
                        distanceZ += gridIndicator.gridZ[k];
                    }
                    distanceZ = 0;
                    distanceY += gridIndicator.gridY[j];
                }
                distanceY = 0;
                distanceX += gridIndicator.gridX[i];
            }
            int[] tris = gridIndicator.mesh.triangles;
            Vector3[] verts = gridIndicator.mesh.vertices;
            ShowVertices(gridIndicator.transform.position + gridIndicator.meshPosition, verts);
            ShowWireFrame(gridIndicator.transform.position + gridIndicator.meshPosition,verts, tris);
        }

        //Draws the vertices of the mesh that will be sliced
        public void ShowVertices(Vector3 pos, Vector3[] verts)
        {
            Handles.color = Color.white;
            float size = HandleUtility.GetHandleSize(pos);
            for (int i = 0; i < verts.Length; i++)
            {
                Handles.CubeCap(0, pos + Quaternion.Euler(gridIndicator.meshRotation) * Vector3.Scale(verts[i], gridIndicator.meshScale), Quaternion.identity, size * 0.05f);
            }
        }

        //Draws the triangles of the mesh that will be sliced
        public void ShowWireFrame(Vector3 pos,Vector3[] verts, int[] tris)
        {
            Handles.color = Color.grey;
            for (int i = 0; i < tris.Length; i += 3)
            {
                Handles.DrawLine(pos +Quaternion.Euler(gridIndicator.meshRotation) * Vector3.Scale(verts[tris[i]], gridIndicator.meshScale), pos +Quaternion.Euler(gridIndicator.meshRotation) * Vector3.Scale(verts[tris[i+1]], gridIndicator.meshScale));
                Handles.DrawLine(pos +Quaternion.Euler(gridIndicator.meshRotation) * Vector3.Scale(verts[tris[i+1]], gridIndicator.meshScale), pos +Quaternion.Euler(gridIndicator.meshRotation) * Vector3.Scale(verts[tris[i + 2]], gridIndicator.meshScale));
                Handles.DrawLine(pos +Quaternion.Euler(gridIndicator.meshRotation) * Vector3.Scale(verts[tris[i+2]], gridIndicator.meshScale), pos +Quaternion.Euler(gridIndicator.meshRotation) * Vector3.Scale(verts[tris[i]], gridIndicator.meshScale));
              }
        }
    }
}

