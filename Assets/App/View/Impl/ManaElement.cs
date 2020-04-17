namespace App.View.Impl
{
    using Dekuple.View.Impl;
    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// View of a Mana piece in the main game scene.
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
