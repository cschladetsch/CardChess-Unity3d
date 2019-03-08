using Dekuple.View.Impl;
using UnityEngine;

namespace App.View.Impl1
{
    /// <inheritdoc />
    /// <summary>
    /// View of a mana piece in the main game scene
    /// </summary>
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
