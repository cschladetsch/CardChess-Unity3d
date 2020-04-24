using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to view 3D mesh object in play mode
//Drag this script on empty object and add a SolidMesh mesh to generatedMesh property
namespace Cogobyte.ProceduralLibrary
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshObject : MonoBehaviour
    {
        public ProceduralMesh generatedMesh;

        void Update()
        {
            DestroyImmediate(GetComponent<MeshFilter>().mesh, false);
            generatedMesh.generate();
            GetComponent<MeshFilter>().mesh = generatedMesh.mesh;
        }


    }
}