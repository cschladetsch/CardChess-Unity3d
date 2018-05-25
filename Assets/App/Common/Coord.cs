using UnityEngine.Assertions;

namespace App.Common
{
    /// <summary>
    /// A coordinate on the board.
    /// </summary>
    public struct Coord
    {
        public readonly int x;
        public readonly int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int ManhattanDistance(Coord other)
        {
            var d = this - other;
            var dx = System.Math.Abs(d.x);
            var dy = System.Math.Abs(d.y);
            return dx + dy;
        }

        public static int ManhattanDistance(Coord a, Coord b)
        {
            return a.ManhattanDistance(b);
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.x + b.x, a.y + b.y);
        }

        public static Coord operator -(Coord a, Coord b)
        {
            return new Coord(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Coord c1, Coord c2)
        {
            return !c1.Equals(c2);
        }

        public int MaxOrthoDistance(Coord other)
        {
            var d = this - other;
            return System.Math.Max(System.Math.Abs(d.x), System.Math.Abs(d.y));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coord))
                return false;
            var c = (Coord)obj;
            return c.x == x && c.y == y;
        }

        public override int GetHashCode()
        {
            return (x * 137) ^ (y * 199);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
