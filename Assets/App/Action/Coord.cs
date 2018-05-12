using UnityEngine.Assertions;

namespace App.Action
{
    public class Coord : Vector2
    {
        public Coord(int x, int y) : base(x,y)
        {
            Assert.IsTrue(x >= 0);
            Assert.IsTrue(y >= 0);
            Assert.IsTrue(x < 20);
            Assert.IsTrue(y < 20);

            X = x;
            Y = y;
        }
    }
}
