using System;
using System.Diagnostics;
using System.Linq;
using App.Action;
using App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App
{
    //[TestClass]
    //public class TestGame
    //{
        //[TestMethod]
        //public void TestArcherMovement()
        //{
        //    var arbiter = RandomBasicSetup<MockWhitePlayerAgent, MockBlackPlayerAgent>();
        //    var board = arbiter.BoardAgent;
        //    var card = arbiter.NewCardAgent(ECardType.Archer, arbiter.WhitePlayerAgent);

        //    var coord = new Coord(3, 3);
        //    board.PlaceCard(card, coord);
        //    var squares = board.GetMovements(coord);
        //    var text = board.ToString(
        //        (c) => squares.Contains(c) ? "x " : $"{board.CardToRep(board.At(c))}");
        //    Trace.WriteLine(text);
        //}

        /*
        [TestMethod]
        public void TestPlayKings()
        {
            var arbiter = RandomBasicSetup<MockWhitePlayerAgent, MockBlackPlayerAgent>();

            arbiter.NewGame();

            StepArbiter(2);
            Trace.WriteLine(Arbiter.Kernel.Root);

            var w = arbiter.WhitePlayerAgent;
            var b = arbiter.BlackPlayerAgent;

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
        */
    //}
}
