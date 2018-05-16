namespace App.Action
{
    public class Vector2
    {
        public int x;
        public int y;

        public Vector2()
        {
        }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public override bool Equals(object obj)
        {
            var v = obj as Vector2;
            return v != null && Equals(v);
        }

        protected bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }
    }
}
