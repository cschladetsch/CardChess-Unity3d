using App.Common;
using UnityEngine;

#pragma warning disable 649

namespace App.View.Impl
{
    class SquareView
        : MonoBehaviour
        , ISquareView
    {
        public EColor Color { get; set; }
        public Coord Coord { get; internal set; }
        public float Length;

        public override int GetHashCode()
        {
            return Coord.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var sq = other as SquareView;
            if (sq == null)
                return false;
            return Coord == sq.Coord;
        }

        public override string ToString()
        {
            return $"{Color} square at {Coord}";
        }
    }
}
