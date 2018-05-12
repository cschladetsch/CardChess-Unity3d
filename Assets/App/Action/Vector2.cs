namespace App.Action
{
    public class Vector2
    {
        public int X;
        public int Y;

        public Vector2()
        {
        }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public override bool Equals(object obj)
        {
            var v = obj as Vector2;
            return v != null && Equals(v);
        }

        protected bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}
