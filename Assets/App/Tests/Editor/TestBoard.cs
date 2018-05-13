using System;
using NUnit.Framework;

namespace App
{
    using Action;

    [TestFixture]
    public class TestBoard
    {
        [TestCase(4,4)]
        [TestCase(8,8)]
        public void TestBoardModel(int width, int height)
        {
            var board = new Model.Board();
            board.Create(width, height);
            Assert.AreEqual(board.Width, width);
            Assert.AreEqual(board.Height, height);
            Assert.IsTrue(board.IsValidCoord(new Coord(0, 0)));
            Assert.IsTrue(board.IsValidCoord(new Coord(width - 1, height - 1)));
            Assert.IsFalse(board.IsValidCoord(new Coord(width, height)));

            var squares = 0;
            foreach (var square in board.GetContents())
            {
                Assert.IsNull(square);
                ++squares;
            }
            Assert.AreEqual(squares, width * height);
        }

        [Test]
        public void TestBoardSetup()
        {
            var arbiter = new Arbiter();
            var b0 = arbiter.NewModel<Model.Board, int, int>(8, 8);
            var c0 = arbiter.NewAgent<Agent.Board, Model.IBoard>(b0);
            var m0 = arbiter.NewModel<Model.Player, EColor>(EColor.White);
            var m1 = arbiter.NewModel<Model.Player, EColor>(EColor.Black);
            var p0 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(m0);
            var p1 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(m1);
            var d0 = arbiter.NewModel<Model.Deck, Guid, Model.IPlayer>(Guid.Empty, m0);
            var d1 = arbiter.NewModel<Model.Deck, Guid, Model.IPlayer>(Guid.Empty, m1);

            m0.SetDeck(d0);
            m1.SetDeck(d1);

            arbiter.Setup(c0, p0, p1);

            Assert.IsNotNull(p0);
            Assert.IsNotNull(p0.Model);
            Assert.IsNotNull(m0.Hand);
            Assert.IsNotNull(m0.Deck);
            Assert.IsNotNull(m0.Deck.Cards);
            Assert.AreSame(p0.Model, m0);

            Assert.AreEqual(7, p0.Model.Hand.Cards.Count);
            Assert.AreEqual(7, p1.Model.Hand.Cards.Count);
            Assert.AreEqual(43, d0.Cards.Count);
            Assert.AreEqual(43, d1.Cards.Count);
        }
    }
}
