using Dekuple.View.Impl;
using UnityEngine;

namespace App.View.Impl1
{
    public class ManaElement
        : ViewBase
    {
        public float Width;

        public Material ActiveMaterial;
        public Material InactiveMaterial;

        public void SetAvailable(bool active)
        {
            GetComponent<Renderer>().material = active ? ActiveMaterial : InactiveMaterial;
        }
    }
}
