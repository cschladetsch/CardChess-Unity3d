using System;
using System.Diagnostics;
using System.Linq;
using App.Action;
using App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App
{
    [TestClass]
    public class TestGame : TestGameBase
    {
        [TestMethod]
        public void TestArcherMovement()
        {
            var arbiter = RandomBasicSetup<MockWhitePlayer, MockBlackPlayer>();
            var board = arbiter.Board;
            var card = arbiter.NewCardAgent(ECardType.Archer, arbiter.WhitePlayer);

            var coord = new Coord(3, 3);
            board.PlaceCard(card, coord);
            var squares = board.GetMovements(coord);
            var text = board.ToString(
                (c) => squares.Contains(c) ? "x " : $"{board.CardToRep(board.At(c))}");
            Trace.WriteLine(text);
        }

        [TestMethod]
        public void TestPlayKings()
        {
            var arbiter = RandomBasicSetup<MockWhitePlayer, MockBlackPlayer>();

            arbiter.NewGame();

            StepArbiter(2);
            Trace.WriteLine(Arbiter.Kernel.Root);

            var w = arbiter.WhitePlayer;
            var b = arbiter.BlackPlayer;

            StepArbiter(2);
            Trace.WriteLine(Arbiter.Kernel.Root);

            w.AcceptCards();
            b.AcceptCards();

            StepArbiter(2);
            Trace.WriteLine(Arbiter.Kernel.Root);

            w.PlaceKing(new Coord(3, 1));
            b.PlaceKing(new Coord(4, 6));

            StepArbiter(2);
            Trace.WriteLine(Arbiter.Kernel.Root);

            Assert.AreEqual(Parameters.StartHandCardCount, w.Hand.NumCards);
            Assert.AreEqual(Parameters.StartHandCardCount, b.Hand.NumCards);

            Trace.WriteLine(arbiter);
        }
    }
}
