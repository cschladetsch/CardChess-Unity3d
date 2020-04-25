using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cogobyte.ProceduralLibrary
{
    [CustomEditor(typeof(BezierPathArray))]
    public class BezierPathArrayEditor : PathArrayEditor
    {
        private const int stepsPerCurve = 10;
        private const float directionScale = 0.5f;
        private const float handleSize = 0.04f;
        private const float pickSize = 0.06f;

        private BezierSpline spline;
        private int selectedIndex = -1;
        SerializedProperty levelOfDetail;
        SerializedProperty translation;
        SerializedProperty rotation;
        SerializedProperty scale;
        SerializedProperty obstacleCheck;
        SerializedProperty obstacleCheckDirection;
        SerializedProperty obstacleLayer;
        SerializedProperty distanceFromGround;

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            pathArray = target as PathArray;
            spline = (pathArray as BezierPathArray).bezierSpline;
            levelOfDetail = serializedObject.FindProperty("levelOfDetail");
            translation = serializedObject.FindProperty("translation");
            rotation = serializedObject.FindProperty("rotation");
            scale = serializedObject.FindProperty("scale");
            obstacleCheck = serializedObject.FindProperty("obstacleCheck");
            obstacleCheckDirection = serializedObject.FindProperty("obstacleCheckDirection");
            obstacleLayer = serializedObject.FindProperty("obstacleLayer");
            distanceFromGround = serializedObject.FindProperty("distanceFromGround");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(levelOfDetail, new GUIContent("Level Of Detail"));
            EditorGUILayout.PropertyField(translation, new GUIContent("Translation"));
            EditorGUILayout.PropertyField(rotation, new GUIContent("Rotation"));
            EditorGUILayout.PropertyField(scale, new GUIContent("Scale"));
            EditorGUILayout.PropertyField(obstacleCheck, new GUIContent("Obstacle Check"));
            EditorGUILayout.PropertyField(obstacleCheckDirection, new GUIContent("Obstacle Check Direction"));
            EditorGUILayout.PropertyField(obstacleLayer, new GUIContent("Obstacle Layer"));
            EditorGUILayout.PropertyField(distanceFromGround, new GUIContent("Distance From Ground"));
            serializedObject.ApplyModifiedProperties();

            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", spline.loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(pathArray, "Toggle Loop");
                EditorUtility.SetDirty(pathArray);
                spline.loop = loop;
                pathArray.closed = loop;
            }

            if (selectedIndex >= 0 && selectedIndex < spline.points.Count)
            {
                DrawSelectedPointInspector();
                if (spline.points.Count > 2)
                {
                    if (GUILayout.Button("Delete Point"))
                    {
                        Undo.RecordObject(pathArray, "Delete Point");
                        spline.DeleteCurve(selectedIndex);
                        selectedIndex = -1;
                        EditorUtility.SetDirty(pathArray);
                    }
                }
                if (selectedIndex != spline.points.Count - 1)
                {
                    if (GUILayout.Button("Split Curve"))
                    {
                        Undo.RecordObject(pathArray, "Split Curve");
                        spline.SplitCurve(selectedIndex);
                        selectedIndex = -1;
                        EditorUtility.SetDirty(pathArray);
                    }
                }
            }
            if (GUILayout.Button("Add Point"))
            {
                Undo.RecordObject(pathArray, "Add Point");
                spline.AddCurve();
                EditorUtility.SetDirty(pathArray);
            }

        }

        private void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", spline.points[selectedIndex].position);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(pathArray, "Move Point");
                EditorUtility.SetDirty(pathArray);
                spline.points[selectedIndex].MovePoint(point);
            }
            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", spline.points[selectedIndex].controlPointMode);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(pathArray, "Change Point Mode");
                spline.SetControlPointMode(selectedIndex, mode);
                EditorUtility.SetDirty(pathArray);
            }
        }

        public override void OnSceneGUI(SceneView sceneView)
        {
            base.OnSceneGUI(sceneView);
            if (spline.points.Count < 2) spline.Reset();
            for (int i = 0; i < spline.points.Count; i++)
            {
                ShowPoint(i);
                Handles.color = Color.cyan;
                Handles.DrawLine(spline.points[i].position, spline.points[i].backControlPoint);
                Handles.DrawLine(spline.points[i].position, spline.points[i].frontControlPoint);
                if (i == spline.points.Count - 1)
                {
                    if (spline.loop)
                    {
                        Handles.DrawBezier(spline.points[i].position, spline.points[0].position, spline.points[i].frontControlPoint, spline.points[0].backControlPoint, Color.white, null, 2f);
                    }
                }
                else
                {
                    Handles.DrawBezier(spline.points[i].position, spline.points[i + 1].position, spline.points[i].frontControlPoint, spline.points[i + 1].backControlPoint, Color.white, null, 2f);
                }
            }
            ShowDirections();
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            Vector3 point = spline.GetPoint(0f);
            Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
            int steps = stepsPerCurve * spline.CurveCount;
            for (int i = 1; i <= steps; i++)
            {
                point = spline.GetPoint(i / (float)steps);
                Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
            }
        }

        private void ShowPoint(int index)
        {
            Handles.Label(spline.points[index].position, spline.points[index].position.ToString());
            float size = HandleUtility.GetHandleSize(spline.points[index].position);
            Handles.color = Color.green;
            if (Handles.Button(spline.points[index].position, Quaternion.identity, size * handleSize, size * 3 * pickSize, Handles.DotCap))
            {
                selectedIndex = index;
                Repaint();
            }
            if (selectedIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 point = Handles.DoPositionHandle(spline.points[index].position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(pathArray, "Move Point");
                    EditorUtility.SetDirty(pathArray);
                    spline.points[index].MovePoint(point);
                }
                Handles.CubeCap(17, spline.points[index].frontControlPoint, Quaternion.identity, size * handleSize);
                EditorGUI.BeginChangeCheck();
                Vector3 frontControlPoint = Handles.DoPositionHandle(spline.points[index].frontControlPoint, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(pathArray, "Move Front Point");
                    EditorUtility.SetDirty(pathArray);
                    spline.points[index].MoveFrontControlPoint(frontControlPoint);
                }
                Handles.CubeCap(18, spline.points[index].backControlPoint, Quaternion.identity, size * handleSize);
                EditorGUI.BeginChangeCheck();
                Vector3 backControlPoint = Handles.DoPositionHandle(spline.points[index].backControlPoint, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(pathArray, "Move Back Point");
                    EditorUtility.SetDirty(pathArray);
                    spline.points[index].MoveBackControlPoint(backControlPoint);
                }

            }
        }
    }
}
