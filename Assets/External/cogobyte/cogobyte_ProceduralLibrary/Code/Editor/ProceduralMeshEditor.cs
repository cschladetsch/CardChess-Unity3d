using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cogobyte.ProceduralLibrary
{
    [CustomEditor(typeof(ProceduralMesh), true)]
    public class ProceduralMeshEditor : Editor
    {
        public ProceduralMesh proceduralMesh;
        public string assetName = "customMesh";
        public bool showWireframe = true;
        public bool showVertices = true;
        public bool showNormals = false;

        void OnEnable()
        {
            proceduralMesh = target as ProceduralMesh;
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Focus On Object Postion"))
            {
                SceneView sceneView = (SceneView)SceneView.sceneViews[0];
                sceneView.LookAt(proceduralMesh.position, Quaternion.Euler(45, 45, 0));
            }
            DrawDefaultInspector();
            if (Conditions())
            {
                if (GUILayout.Button("Save As Asset"))
                {
                    ProceduralMesh meshCopy = Object.Instantiate(proceduralMesh) as ProceduralMesh;
                    SetMeshParameters(meshCopy);
                    meshCopy.generate();
                    assetName = EditorUtility.SaveFilePanelInProject("Save Mesh", assetName, "asset", "Please enter a file name to save the mesh to");
                    UnityEditor.AssetDatabase.CreateAsset(meshCopy.mesh, assetName);
                }
                showVertices = EditorGUILayout.Toggle("Show Vertices", showVertices);
                showWireframe = EditorGUILayout.Toggle("Show Wireframe", showWireframe);
                showNormals = EditorGUILayout.Toggle("Show Normals", showNormals);
                SceneView sceneView = (SceneView)SceneView.sceneViews[0];
                sceneView.Repaint();
            }
        }

        public virtual bool Conditions()
        {
            return true;
        }

        public virtual void SetMeshParameters(ProceduralMesh meshCopy)
        {

        }

        public virtual void OnSceneGUI(SceneView sceneView)
        {
            if (Conditions() && (showNormals || showVertices || showWireframe))
            {
                ProceduralMesh meshCopy = Object.Instantiate(proceduralMesh) as ProceduralMesh;
                SetMeshParameters(meshCopy);
                meshCopy.generate();
                if (showVertices)
                {
                    ShowVertices(meshCopy.position, meshCopy.mesh.vertices, meshCopy.mesh.colors32);

                }
                if (showWireframe)
                {
                    ShowWireFrame(meshCopy.mesh.vertices, meshCopy.mesh.triangles);
                }
                if (showNormals)
                {
                    ShowNormals(meshCopy.mesh.vertices, meshCopy.mesh.normals);

                }
                DestroyImmediate(meshCopy.mesh, true);
                DestroyImmediate(meshCopy, true);
            }
        }

        public static void ShowVertices(Vector3 pos, Vector3[] verts, Color32[] colors)
        {
            float size = HandleUtility.GetHandleSize(pos);
            for (int i = 0; i < verts.Length; i++)
            {
                Handles.color = colors[i];
                Handles.CubeCap(0, verts[i], Quaternion.identity, size * 0.05f);
            }
        }

        public static void ShowWireFrame(Vector3[] verts, int[] tris)
        {
            Handles.color = Color.grey;
            for (int i = 0; i < tris.Length; i += 3)
            {
                Handles.DrawLine(verts[tris[i]], verts[tris[i + 1]]);
                Handles.DrawLine(verts[tris[i + 1]], verts[tris[i + 2]]);
                Handles.DrawLine(verts[tris[i + 2]], verts[tris[i]]);
            }
        }

        public static void ShowNormals(Vector3[] verts, Vector3[] normals)
        {
            Handles.color = Color.red;
            for (int i = 0; i < normals.Length; i++)
            {
                Handles.DrawLine(verts[i], verts[i] + normals[i]);
            }
        }
    }
}
