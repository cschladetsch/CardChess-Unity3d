using UnityEditor;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    [CustomEditor(typeof(PathArray), true)]
    public class PathArrayEditor : Editor
    {
        public PathArray pathArray;
        
        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            pathArray = target as PathArray;
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        public virtual void OnSceneGUI(SceneView sceneView)
        {
            pathArray.CalculatePath();
            Handles.color = Color.magenta;
            for (int i = 0; i < pathArray.path.Count-1; i++)
            {
                Handles.DrawLine(pathArray.path[i], pathArray.path[i+1]);
            }
        }
    }
}
