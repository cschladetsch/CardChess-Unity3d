using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Outline item made of two points
    public class Outline
    {
        public Outline(int first, int second, int inside)
        {
            firstIndex = first;
            secondIndex = second;
            this.inside = inside;
            closingVertice = 0;
        }
        public int firstIndex;//first vertex
        public int secondIndex;//second vertex
        public int inside;//0 outside 1,2,3,... inside holes
        public int closingVertice;//Index of first vertex in outline if closed;-1 if not;
    }
}
