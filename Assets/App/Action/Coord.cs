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
            Assert.IsTrue(x >= 0);
            Assert.IsTrue(y >= 0);
            Assert.IsTrue(x < 20);
            Assert.IsTrue(y < 20);

            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var c = obj as Coord;
            if (c == null)
                return false;
            return X == c.X && Y == c.Y;
        }

        public override int GetHashCode()
        {
            return (X*137) ^ (Y*199);
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }
    }
}
