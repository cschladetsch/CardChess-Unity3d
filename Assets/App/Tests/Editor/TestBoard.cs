using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

using App.Model;

namespace App
{
    [TestFixture]
    public class TestBoard
    {
        [TestCase(4,4)]
        [TestCase(8,8)]
        public void TestCreate(int width, int height)
        {
            Board board = new Model.Board(width, height);
            Assert.AreEqual(board.Width, width);
            Assert.AreEqual(board.Height, height);
            Assert.IsTrue(board.IsValid(new Coord(0, 0)));
            Assert.IsTrue(board.IsValid(new Coord(width - 1, height - 1)));
            Assert.IsFalse(board.IsValid(new Coord(width, height)));

            int squares = 0;
            foreach (var square in board.GetContents())
            {
                Assert.IsNull(square);
                ++squares;
            }
            Assert.AreEqual(squares, width * height);
        }
    }
}
