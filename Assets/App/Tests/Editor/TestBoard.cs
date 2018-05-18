using System;
using NUnit.Framework;

namespace App
{
    using Common;
    using Model;
    using Action;

    [TestFixture]
    public class TestBoard
    {
        [TestCase(4,4)]
        [TestCase(8,8)]
        public void TestBoardModel(int width, int height)
        {
            var board = new BoardModel(width, height);
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
            //var b0 = arbiter.NewModel<BoardModel, int, int>(8, 8);
            var b0 = new BoardModel(8,8);
            var c0 = arbiter.NewAgent<Agent.BoardAgent, IBoardModel>(b0);
            var m0 = arbiter.NewModel<PlayerModel, EColor>(EColor.White);
            var m1 = arbiter.NewModel<PlayerModel, EColor>(EColor.Black);
            var p0 = arbiter.NewAgent<Agent.PlayerAgent, IPlayerModel>(m0);
            var p1 = arbiter.NewAgent<Agent.PlayerAgent, IPlayerModel>(m1);
            var d0 = arbiter.NewModel<DeckModel, Guid, IPlayerModel>(Guid.Empty, m0);
            var d1 = arbiter.NewModel<DeckModel, Guid, IPlayerModel>(Guid.Empty, m1);

            m0.SetDeck(d0);
            m1.SetDeck(d1);

            arbiter.Setup(c0, p0, p1);

            m0.MockMakeHand();
            m1.MockMakeHand();

            Assert.IsNotNull(p0);
            Assert.IsNotNull(p0.Model);
            Assert.IsNotNull(m0.HandModel);
            Assert.IsNotNull(m0.DeckModel);
            Assert.AreSame(p0.Model, m0);

            Assert.AreEqual(Parameters.StartHandCardCount, p0.Deck.NumCards);
            Assert.AreEqual(Parameters.StartHandCardCount, p1.Deck.NumCards);
            Assert.AreEqual(Parameters.MaxCardsInDeck - Parameters.StartHandCardCount, d0.NumCards);
            Assert.AreEqual(Parameters.MaxCardsInDeck - Parameters.StartHandCardCount, d1.NumCards);
        }
    }
}
