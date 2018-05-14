using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Main;

namespace App
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBasicPrint()
        {
            var k = Flow.Create.Kernel();
            var f = k.Factory;

            var t0 = f.Transient("Trans0");
            var t1 = f.Transient("Trans1");
            var t2 = f.Transient("Trans2");
            var r0 = f.Trigger("Trigger0", t2, t1);
            var b0 = f.Barrier("Barrier0", t0, r0);
            var f0 = f.NamedFuture<int>("Future<int>");
            var g0 = f.Group("Group0", t0, f0, b0);

            k.Root.Add(g0);

            // allow all objects to be placed
            for (int n = 0; n < 10; ++n)
                k.Step();

            var s0 = Flow.Logger.PrettyPrinter.ToString(g0);
            Console.WriteLine(s0);
        }

        private static Arbiter BasicSetup<TPlayer0, TPlayer1>()
            where TPlayer0 : class, Agent.IPlayer, new()
            where TPlayer1 : class, Agent.IPlayer, new()
        {
            var arbiter = new Arbiter();
            var b0 = arbiter.NewModel<Model.Board, int, int>(8, 8);
            var c0 = arbiter.NewAgent<Agent.Board, Model.IBoard>(b0);

            var m0 = arbiter.NewModel<Model.Player, EColor>(EColor.White);
            var m1 = arbiter.NewModel<Model.Player, EColor>(EColor.Black);
            var p0 = arbiter.NewAgent<TPlayer0, Model.IPlayer>(m0);
            var p1 = arbiter.NewAgent<TPlayer1, Model.IPlayer>(m1);

            var d0 = arbiter.NewModel<Model.Deck, Guid, Model.IPlayer>(Guid.Empty, m0);
            var d1 = arbiter.NewModel<Model.Deck, Guid, Model.IPlayer>(Guid.Empty, m1);

            m0.SetDeck(d0);
            m1.SetDeck(d1);

            arbiter.Setup(c0, p0, p1);

            return arbiter;
        }

        private static void Step(Flow.IGenerator gen, int steps)
        {
            for (var n = 0; n < steps; ++n)
                gen.Step();
        }

        [TestMethod]
        public void TestBoardSetup()
        {
            var arbiter = BasicSetup<Agent.Player, Agent.Player>();

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

            Assert.AreEqual(7, p0.Model.Hand.Cards.Count);
            Assert.AreEqual(7, p1.Model.Hand.Cards.Count);
            Assert.AreEqual(43, d0.Cards.Count);
            Assert.AreEqual(43, d1.Cards.Count);
        }

        [TestMethod]
        public void TestPlayKings()
        {
            var arbiter = BasicSetup<MockWhitePlayer, MockBlackPlayer>();
            arbiter.StartGame();
            Step(Arbiter.Kernel, 10);
        }
    }
}
