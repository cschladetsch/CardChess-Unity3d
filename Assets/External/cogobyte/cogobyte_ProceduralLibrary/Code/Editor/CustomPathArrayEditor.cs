using UnityEditor;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    [CustomEditor(typeof(CustomPathArray), true)]
    public class CustomPathArrayEditor : PathArrayEditor
    {
        bool needsRepaint;
        private int selectedIndex = -1;
        private const float handleSize = 0.1f;
        private const float pickSize = 0.08f;

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            pathArray = target as CustomPathArray;
        }

        public override void OnInspectorGUI()
        {
            CustomPathArray customPath = pathArray as CustomPathArray;
            DrawDefaultInspector();
            EditorGUI.BeginChangeCheck();
            if (selectedIndex >= 0 && selectedIndex < customPath.customPath.Count)
            {
                if (customPath.customPath.Count > 2)
                {
                    if (GUILayout.Button("Delete Point"))
                    {
                        Undo.RecordObject(pathArray, "Delete Point");
                        customPath.customPath.RemoveAt(selectedIndex);
                        selectedIndex = -1;
                        EditorUtility.SetDirty(pathArray);
                    }
                }
                if (selectedIndex != customPath.customPath.Count - 1)
                {
                    if (GUILayout.Button("Split Line"))
                    {
                        Undo.RecordObject(pathArray, "Split Curve");
                        customPath.customPath.Insert(selectedIndex + 1, (customPath.customPath[selectedIndex] + customPath.customPath[selectedIndex+1])/2);
                        selectedIndex = -1;
                        EditorUtility.SetDirty(pathArray);
                    }
                }
            }
            EditorGUI.EndChangeCheck();
            EditorUtility.SetDirty(pathArray);
        }

        public override void OnSceneGUI(SceneView sceneView)
        {
            CustomPathArray customPath = pathArray as CustomPathArray;
            for (int i=0;i< customPath.customPath.Count-1; i++)
            {
                DrawPoint(i, i + 1);
            }
            DrawPoint(customPath.customPath.Count - 1, 0);
            base.OnSceneGUI(sceneView);
        }

        void DrawPoint(int p1,int p2)
        {
            CustomPathArray customPath = pathArray as CustomPathArray;
            float size = HandleUtility.GetHandleSize(customPath.customPath[p1]);
            if (Handles.Button(customPath.customPath[p1], Quaternion.identity, size * handleSize, size * pickSize, Handles.DotCap))
            {
                selectedIndex = p1;
                Repaint();
            }
            if(!(!customPath.closed && p2==0)) Handles.DrawLine(customPath.customPath[p1], customPath.customPath[p2]);
            if (selectedIndex == p1)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 point = Handles.DoPositionHandle(customPath.customPath[p1], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(pathArray, "Move Point");
                    EditorUtility.SetDirty(pathArray);
                    customPath.customPath[p1] = point;
                }
            }
        }

   



    }
}
