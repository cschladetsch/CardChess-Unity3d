using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cogobyte.ProceduralIndicators
{
    //Editor for the grid indicator asset
    [CustomEditor(typeof(GridIndicator))]
    [CanEditMultipleObjects]
    public class GridIndicatorEditor : Editor
    {
        public GridIndicator gridIndicator;

        void OnEnable()
        {
            gridIndicator = target as GridIndicator;
        }

        public virtual void OnSceneGUI()
        {
            if (gridIndicator.gridX.Length == 0) { gridIndicator.gridX = new float[1] { 1 }; }
            if (gridIndicator.gridY.Length == 0) { gridIndicator.gridY = new float[1] { 1 }; }
            if (gridIndicator.gridZ.Length == 0) { gridIndicator.gridZ = new float[1] { 1 }; }
            gridIndicator.Generate();
        }
    }
}