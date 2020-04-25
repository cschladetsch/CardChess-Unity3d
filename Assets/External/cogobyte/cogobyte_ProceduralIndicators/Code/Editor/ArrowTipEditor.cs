using Cogobyte.ProceduralLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Editor For ArrowTip Asset
    [CustomEditor(typeof(ArrowTip))]
    [CanEditMultipleObjects]
    public class ArrowTipEditor : Editor
    {
        SerializedProperty arrowTipMode;
        SerializedProperty size;
        SerializedProperty levelOfDetailAlongPath;
        SerializedProperty arrowTipPathType;
        SerializedProperty pathAlongXFunctionLength;
        SerializedProperty pathAlongXFunction;
        SerializedProperty pathAlongYFunctionLength;
        SerializedProperty pathAlongYFunction;
        SerializedProperty pathAlongZFunctionLength;
        SerializedProperty pathAlongZFunction;
        SerializedProperty widthFunctionLength;
        SerializedProperty widthFunction;
        SerializedProperty heightFunctionLength;
        SerializedProperty heightFunction;
        SerializedProperty rotationFunctionLength;
        SerializedProperty rotateFunction;
        SerializedProperty templatePrimitives;
        SerializedProperty shapeFunctionLength;
        SerializedProperty shapeFunction;
        SerializedProperty mesh;
        public string assetName = "customMesh";
        public bool showWireframe = true;
        public bool showVertices = true;
        public bool showNormals = false;

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            arrowTipMode = serializedObject.FindProperty("arrowTipMode");
            size = serializedObject.FindProperty("size");
            levelOfDetailAlongPath = serializedObject.FindProperty("levelOfDetailAlongPath");
            arrowTipPathType = serializedObject.FindProperty("arrowTipPathType");
            pathAlongXFunctionLength = serializedObject.FindProperty("pathAlongXFunctionLength");
            pathAlongXFunction = serializedObject.FindProperty("pathAlongXFunction");
            pathAlongYFunctionLength = serializedObject.FindProperty("pathAlongYFunctionLength");
            pathAlongYFunction = serializedObject.FindProperty("pathAlongYFunction");
            pathAlongZFunctionLength = serializedObject.FindProperty("pathAlongZFunctionLength");
            pathAlongZFunction = serializedObject.FindProperty("pathAlongZFunction");
            widthFunctionLength = serializedObject.FindProperty("widthFunctionLength");
            widthFunction = serializedObject.FindProperty("widthFunction");
            heightFunctionLength = serializedObject.FindProperty("heightFunctionLength");
            heightFunction = serializedObject.FindProperty("heightFunction");
            rotationFunctionLength = serializedObject.FindProperty("rotationFunctionLength");
            rotateFunction = serializedObject.FindProperty("rotateFunction");
            templatePrimitives = serializedObject.FindProperty("templatePrimitives");
            shapeFunctionLength = serializedObject.FindProperty("shapeFunctionLength");
            shapeFunction = serializedObject.FindProperty("shapeFunction");
            mesh = serializedObject.FindProperty("mesh");
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(arrowTipMode, new GUIContent("Endpoint Mode"));
            if (arrowTipMode.enumValueIndex != (int)ArrowTip.ArrowTipMode.None)
            {
                EditorGUILayout.PropertyField(size, new GUIContent("Size"));
                if (arrowTipMode.enumValueIndex != (int)ArrowTip.ArrowTipMode.Mesh)
                {
                    EditorGUILayout.PropertyField(arrowTipPathType, new GUIContent("Path Mode"));
                    if (arrowTipPathType.enumValueIndex == (int)ArrowPath.ArrowPathType.Function)
                    {
                        EditorGUILayout.PropertyField(levelOfDetailAlongPath, new GUIContent("Level Of Detail Along Path"));
                        EditorGUILayout.PropertyField(pathAlongXFunctionLength, new GUIContent("Path Along X Function Length"));
                        EditorGUILayout.PropertyField(pathAlongXFunction, new GUIContent("Path Along X Function"));
                        EditorGUILayout.PropertyField(pathAlongYFunctionLength, new GUIContent("Path Along Y Function Length"));
                        EditorGUILayout.PropertyField(pathAlongYFunction, new GUIContent("Path Along Y Function"));
                        EditorGUILayout.PropertyField(pathAlongZFunctionLength, new GUIContent("Path Along Z Function Length"));
                        EditorGUILayout.PropertyField(pathAlongZFunction, new GUIContent("Path Along Z Function"));
                    }
                    EditorGUILayout.PropertyField(widthFunctionLength, new GUIContent("Width Function Length"));
                    EditorGUILayout.PropertyField(widthFunction, new GUIContent("Width Function"));
                    EditorGUILayout.PropertyField(heightFunctionLength, new GUIContent("Height Function Length"));
                    EditorGUILayout.PropertyField(heightFunction, new GUIContent("Height Function"));
                    EditorGUILayout.PropertyField(rotationFunctionLength, new GUIContent("Rotation Function Length"));
                    EditorGUILayout.PropertyField(rotateFunction, new GUIContent("Rotation Function"));
                    EditorGUILayout.PropertyField(templatePrimitives, new GUIContent("Template Primitives"));
                    EditorGUILayout.PropertyField(shapeFunctionLength, new GUIContent("Shape Function Length"));
                    EditorGUILayout.PropertyField(shapeFunction, new GUIContent("Shape Function"));
                }
                else
                {
                    EditorGUILayout.PropertyField(mesh, new GUIContent("Mesh"));
                }
            }
            serializedObject.ApplyModifiedProperties();
            ArrowIndicator arrowIndicator = InitArrowIndicator();

            if (GUILayout.Button("Save As Asset"))
            {
                arrowIndicator.generate();
                assetName = EditorUtility.SaveFilePanelInProject("Save Mesh", assetName, "asset", "Please enter a file name to save the mesh to");
                UnityEditor.AssetDatabase.CreateAsset(arrowIndicator.mesh, assetName);
            }
            showVertices = EditorGUILayout.Toggle("Show Vertices", showVertices);
            showWireframe = EditorGUILayout.Toggle("Show Wireframe", showWireframe);
            showNormals = EditorGUILayout.Toggle("Show Normals", showNormals);
        }

        public virtual void OnSceneGUI(SceneView sceneView)
        {
            if (showNormals || showVertices || showWireframe)
            {
                ArrowIndicator arrowIndicator = InitArrowIndicator();
                arrowIndicator.generate();
                if (showVertices)
                {
                    ProceduralMeshEditor.ShowVertices(arrowIndicator.position, arrowIndicator.mesh.vertices, arrowIndicator.mesh.colors32);

                }
                if (showWireframe)
                {
                    ProceduralMeshEditor.ShowWireFrame(arrowIndicator.mesh.vertices, arrowIndicator.mesh.triangles);
                }
                if (showNormals)
                {
                    ProceduralMeshEditor.ShowNormals(arrowIndicator.mesh.vertices, arrowIndicator.mesh.normals);
                }
                DestroyImmediate(arrowIndicator.mesh, true);
                DestroyImmediate(arrowIndicator, true);
            }

        }

        public ArrowIndicator InitArrowIndicator()
        {
            ArrowIndicator arrowIndicator = ScriptableObject.CreateInstance<ArrowIndicator>();
            arrowIndicator.arrowPath = ScriptableObject.CreateInstance<ArrowPath>();
            arrowIndicator.arrowPath.arrowPathMode = ArrowPath.ArrowPathMode.None;
            arrowIndicator.arrowPath.arrowHead = target as ArrowTip;
            arrowIndicator.arrowPath.arrowTail = ScriptableObject.CreateInstance<ArrowTip>();
            arrowIndicator.arrowPath.arrowTail.arrowTipMode = ArrowTip.ArrowTipMode.None;
            arrowIndicator.extrudeObject = ScriptableObject.CreateInstance<ComplexExtrude>();
            return arrowIndicator;
        }
    }
}