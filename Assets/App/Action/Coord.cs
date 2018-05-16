using UnityEngine.Assertions;

namespace App.Action
{
    public class Coord : Vector2
    {
        public Coord()
        {
        }

        public Coord(int x, int y) : base(x,y)
        {
        }

        public override bool Equals(object obj)
        {
            var c = obj as Coord;
            if (c == null)
                return false;
            return x == c.x && y == c.y;
        }

        public override int GetHashCode()
        {
            return (x*137) ^ (y*199);
        }

        public override string ToString()
        {
            return $"x:{x} y:{y}";
        }
    }
}
