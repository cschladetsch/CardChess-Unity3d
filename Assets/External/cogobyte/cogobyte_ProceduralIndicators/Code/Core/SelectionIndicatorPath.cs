using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    //Abstract class for SelectionIndicatorPaths
    [System.Serializable]
    public class SelectionIndicatorPath : SolidMesh
    {
        public PathArray pathArray;

        public void prepareGradient(ref Gradient grad)
        {
            grad = new Gradient();
            GradientColorKey[] gck;
            GradientAlphaKey[] gak;
            gck = new GradientColorKey[2];
            gck[0].color = Color.white;
            gck[0].time = 0.0F;
            gck[1].color = Color.white;
            gck[1].time = 1.0F;
            gak = new GradientAlphaKey[2];
            gak[0].alpha = 1.0F;
            gak[0].time = 0.0F;
            gak[1].alpha = 1.0F;
            gak[1].time = 1.0F;
            grad.SetKeys(gck, gak);
        }
    }
}
