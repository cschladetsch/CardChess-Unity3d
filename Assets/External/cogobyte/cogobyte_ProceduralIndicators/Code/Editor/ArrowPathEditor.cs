using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    //Editor For ArrowPath Asset
    [CustomEditor(typeof(ArrowPath))]
    [CanEditMultipleObjects]
    public class ArrowPathEditor : Editor
    {
        SerializedProperty arrowHead;
        SerializedProperty arrowTail;
        SerializedProperty arrowPathMode;
        SerializedProperty arrowPathType;
        SerializedProperty pathArray;
        SerializedProperty levelOfDetailAlongPath;
        SerializedProperty pathAlongXFunctionLength;
        SerializedProperty pathAlongXFunction;
        SerializedProperty pathAlongYFunctionLength;
        SerializedProperty pathAlongYFunction;
        SerializedProperty pathAlongZFunctionLength;
        SerializedProperty pathAlongZFunction;
        SerializedProperty editedPath;
        SerializedProperty widthFunctionLength;
        SerializedProperty widthFunction;
        SerializedProperty heightFunctionLength;
        SerializedProperty heightFunction;
        SerializedProperty rotationFunctionLength;
        SerializedProperty rotateFunction;
        SerializedProperty templatePrimitives;
        SerializedProperty shapeFunctionLength;
        SerializedProperty shapeFunction;
        SerializedProperty customShapes;
        SerializedProperty atPathPoints;
        SerializedProperty distanceBetweenShapes;
        SerializedProperty brokenLineLength;
        SerializedProperty brakeLength;
        SerializedProperty startPoint;
        SerializedProperty endPoint;
        SerializedProperty colorFunctions;
        SerializedProperty useShapeColors;
        public string assetName = "customMesh";
        public bool showWireframe = true;
        public bool showVertices = true;
        public bool showNormals = false;


        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            arrowHead = serializedObject.FindProperty("arrowHead");
            arrowTail = serializedObject.FindProperty("arrowTail");
            arrowPathMode = serializedObject.FindProperty("arrowPathMode");
            levelOfDetailAlongPath = serializedObject.FindProperty("levelOfDetailAlongPath");
            arrowPathType = serializedObject.FindProperty("arrowPathType");
            pathArray = serializedObject.FindProperty("pathArray");
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
            customShapes = serializedObject.FindProperty("customShapes");
            atPathPoints = serializedObject.FindProperty("atPathPoints");
            distanceBetweenShapes = serializedObject.FindProperty("distanceBetweenShapes");
            brokenLineLength = serializedObject.FindProperty("brokenLineLength");
            brakeLength = serializedObject.FindProperty("brakeLength");
            startPoint = serializedObject.FindProperty("startPoint");
            endPoint = serializedObject.FindProperty("endPoint");
            editedPath = serializedObject.FindProperty("editedPath");
            colorFunctions = serializedObject.FindProperty("colorFunctions");
            useShapeColors = serializedObject.FindProperty("useShapeColors");
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(arrowTail, new GUIContent("Arrow Tail"));
            EditorGUILayout.PropertyField(arrowHead, new GUIContent("Arrow Head"));
            EditorGUILayout.PropertyField(arrowPathMode, new GUIContent("Path Mode"));
            EditorGUILayout.PropertyField(arrowPathType, new GUIContent("Path Type"));
            if (arrowPathType.enumValueIndex == (int)ArrowPath.ArrowPathType.Function)
            {
                EditorGUILayout.PropertyField(startPoint, new GUIContent("Start Point"), true);
                EditorGUILayout.PropertyField(endPoint, new GUIContent("End Point"), true);
                EditorGUILayout.PropertyField(pathAlongXFunctionLength, new GUIContent("Path Along X Function Length"));
                EditorGUILayout.PropertyField(pathAlongXFunction, new GUIContent("Path Along X Function"));
                EditorGUILayout.PropertyField(pathAlongYFunctionLength, new GUIContent("Path Along Y Function Length"));
                EditorGUILayout.PropertyField(pathAlongYFunction, new GUIContent("Path Along Y Function"));
                EditorGUILayout.PropertyField(pathAlongZFunctionLength, new GUIContent("Path Along Z Function Length"));
                EditorGUILayout.PropertyField(pathAlongZFunction, new GUIContent("Path Along Z Function"));
            }
            else
            {
                if(arrowPathType.enumValueIndex == (int)ArrowPath.ArrowPathType.PathArray)
                {
                    EditorGUILayout.PropertyField(pathArray, new GUIContent("Path Array"), true);
                    if((target as ArrowPath).pathArray == null)
                    {
                        EditorGUILayout.HelpBox("You need to set the path array", MessageType.Error);
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(editedPath, new GUIContent("Path Points"), true);
                }
            }
            if (arrowPathType.enumValueIndex != (int)ArrowPath.ArrowPathType.PathArray)
            {
                EditorGUILayout.PropertyField(levelOfDetailAlongPath, new GUIContent("Level Of Detail Along Path"));
            }
            if (arrowPathMode.enumValueIndex != (int)ArrowPath.ArrowPathMode.None)
            {
                if (arrowPathMode.enumValueIndex != (int)ArrowPath.ArrowPathMode.MadeOfShapes)
                {
                    EditorGUILayout.PropertyField(widthFunctionLength, new GUIContent("Width Function Length"));
                    EditorGUILayout.PropertyField(widthFunction, new GUIContent("Width Function"));
                    EditorGUILayout.PropertyField(heightFunctionLength, new GUIContent("Height Function Length"));
                    EditorGUILayout.PropertyField(heightFunction, new GUIContent("Height Function"));
                    EditorGUILayout.PropertyField(rotationFunctionLength, new GUIContent("Rotation Function Length"));
                    EditorGUILayout.PropertyField(rotateFunction, new GUIContent("Rotation Function"));
                    EditorGUILayout.PropertyField(templatePrimitives, new GUIContent("Template primitives shape"));
                    EditorGUILayout.PropertyField(shapeFunctionLength, new GUIContent("Shape Function Length"));
                    EditorGUILayout.PropertyField(shapeFunction, new GUIContent("Shape Function"));
                }
                else
                {
                    EditorGUILayout.PropertyField(rotationFunctionLength, new GUIContent("Rotation Function Length"));
                    EditorGUILayout.PropertyField(rotateFunction, new GUIContent("Rotation Function"));
                    EditorGUILayout.PropertyField(customShapes, new GUIContent("Custom Shapes"), true);
                    EditorGUILayout.PropertyField(atPathPoints, new GUIContent("At Path Points"));
                    if (atPathPoints.boolValue == false)
                    {
                        EditorGUILayout.PropertyField(distanceBetweenShapes, new GUIContent("Distances Custom Shapes"), true);
                    }
                }
                if (arrowPathMode.enumValueIndex == (int)ArrowPath.ArrowPathMode.BrokenExtrude)
                {
                        EditorGUILayout.PropertyField(brokenLineLength, new GUIContent("Broken Line length"));
                        EditorGUILayout.PropertyField(brakeLength, new GUIContent("Brake length"));
                }
            }
            EditorGUILayout.PropertyField(colorFunctions, new GUIContent("Gradient"), true);
            EditorGUILayout.PropertyField(useShapeColors, new GUIContent("Use Shape Colors"));
            ArrowIndicator arrowIndicator = InitArrowIndicator();
            if (((target as ArrowPath).arrowPathType != ArrowPath.ArrowPathType.PathArray || (target as ArrowPath).pathArray != null))
            {
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
            serializedObject.ApplyModifiedProperties();
        }

        public virtual void OnSceneGUI(SceneView sceneView)
        {
            if ((showNormals || showVertices || showWireframe) && ((target as ArrowPath).arrowPathType != ArrowPath.ArrowPathType.PathArray || (target as ArrowPath).pathArray != null))
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
            arrowIndicator.arrowPath = Instantiate(target as ArrowPath) as ArrowPath;
            if ((target as ArrowPath).arrowTail == null)
            {
                arrowIndicator.arrowPath.arrowTail = ScriptableObject.CreateInstance<ArrowTip>();
                arrowIndicator.arrowPath.arrowTail.arrowTipMode = ArrowTip.ArrowTipMode.None;
            }
            if ((target as ArrowPath).arrowTail == (target as ArrowPath).arrowHead && (target as ArrowPath).arrowHead!=null)
            {
                arrowIndicator.arrowPath.arrowHead = Object.Instantiate((target as ArrowPath).arrowTail) as ArrowTip;
            }
            else
            {
                if ((target as ArrowPath).arrowHead == null)
                {
                    arrowIndicator.arrowPath.arrowHead = ScriptableObject.CreateInstance<ArrowTip>();
                    arrowIndicator.arrowPath.arrowHead.arrowTipMode = ArrowTip.ArrowTipMode.None;
                }
            }

            arrowIndicator.extrudeObject = ScriptableObject.CreateInstance<ComplexExtrude>();
            return arrowIndicator;
        }
    }
}