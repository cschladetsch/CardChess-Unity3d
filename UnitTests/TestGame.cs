using System.Diagnostics;
using App.Action;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Main;

namespace App
{
    [TestClass]
    public class TestGame : TestGameBase
    {
        [TestMethod]
        public void TestBoardSetup()
        {
            var arbiter = RandomBasicSetup<Agent.Player, Agent.Player>();

            var p0 = arbiter.WhitePlayer;
            var p1 = arbiter.BlackPlayer;
            var m0 = p0.Model;
            var m1 = p1.Model;
            var d0 = m0.Deck;
            var d1 = m1.Deck;

            Assert.IsNotNull(p0);
            Assert.IsNotNull(p0.Model);
            Assert.IsNotNull(m0.Hand);
            Assert.IsNotNull(m0.Deck);
            Assert.IsNotNull(m0.Deck.Cards);
            Assert.IsNotNull(m1.Hand);
            Assert.IsNotNull(m1.Deck);
            Assert.IsNotNull(m1.Deck.Cards);
            Assert.AreSame(p0.Model, m0);
            Assert.AreSame(p1.Model, m1);

            p0.Model.MockMakeHand();
            p1.Model.MockMakeHand();
            Assert.AreEqual(7, p0.Model.Hand.Cards.Count);
            Assert.AreEqual(7, p1.Model.Hand.Cards.Count);
            Assert.AreEqual(43, d0.Cards.Count);
            Assert.AreEqual(43, d1.Cards.Count);
        }

        [TestMethod]
        public void TestPlayKings()
        {
            var arbiter = RandomBasicSetup<MockWhitePlayer, MockBlackPlayer>();
            arbiter.GameLoop();
            StepArbiter(10);

            var w = arbiter.WhitePlayer;
            var b = arbiter.BlackPlayer;

            Trace.WriteLine(Arbiter.Kernel.Root);

            w.AcceptCards();
            b.AcceptCards();

            w.PlaceKing(new Coord(3, 1));
            b.PlaceKing(new Coord(4, 6));

            StepArbiter(10);
            Trace.WriteLine(arbiter);
        }
    }
}
