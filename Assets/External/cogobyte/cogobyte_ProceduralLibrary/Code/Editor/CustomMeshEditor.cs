using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cogobyte.ProceduralLibrary
{
    [CustomEditor(typeof(CustomMesh), true)]
    public class CustomMeshEditor : ProceduralMeshEditor
    {
        public override bool Conditions()
        {
            return (proceduralMesh as CustomMesh).customMesh!=null;
        }
    }
}
