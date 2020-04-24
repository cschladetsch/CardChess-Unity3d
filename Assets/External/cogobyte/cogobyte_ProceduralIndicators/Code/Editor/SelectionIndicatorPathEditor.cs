using UnityEngine;
using UnityEditor;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    [CustomEditor(typeof(SelectionIndicatorPath),true)]
    public class SelectionIndicatorPathEditor : ProceduralMeshEditor
    {
        public override bool Conditions()
        {
            SelectionIndicatorPath selectionIndicatorPath = proceduralMesh as SelectionIndicatorPath;
            if (selectionIndicatorPath.pathArray == null) return false;
            return true;
        }

        public override void SetMeshParameters(ProceduralMesh meshCopy)
        {
            SelectionIndicatorPath copy = meshCopy as SelectionIndicatorPath;
            copy.pathArray = (proceduralMesh as SelectionIndicatorPath).pathArray;
            copy.pathArray.CalculatePath();
        }
    }
}