using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace App.View.Impl1
{
    public class ManaElement
        : ViewBase
    {
        public float Width;

        public Material ActiveMaterial;
        public Material InactiveMaterial;

        public void SetActive(bool active)
        {
            var rend = GetComponent<Renderer>();
            rend.material = active ? ActiveMaterial : InactiveMaterial;
        }
    }
}
