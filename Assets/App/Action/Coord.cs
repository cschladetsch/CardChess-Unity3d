using UnityEngine.Assertions;

namespace App.Action
{
    public struct Coord
    {
		public int x, y;

        public Coord(int x, int y)
        {
			this.x = x;
			this.y = y;
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

		public override bool Equals(object obj)
		{
			if (!(obj is Coord))
				return false;
			var c = (Coord)obj;
			return c.x == x && c.y == y;
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
