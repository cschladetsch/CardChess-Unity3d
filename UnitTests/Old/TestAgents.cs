//using System;
//using System.Collections.Generic;
//using System.Text;

//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace App
//{
//    using Common;
//    using Model;

//    [TestClass]
//    public class TestGameBase
//    {
//        /// <summary>
//        /// Make a basic setup of a boardAgent with two players.
//        /// Each playerAgent has a randomly generated Deck and a Hand
//        /// </summary>
//        /// <typeparam name="TPlayer0">Agent for White playerAgent</typeparam>
//        /// <typeparam name="TPlayer1">Agent for Black playerAgent</typeparam>
//        /// <returns></returns>
//        protected static Arbiter RandomBasicSetup<TPlayer0, TPlayer1>()
//            where TPlayer0 : class, IPlayerAgent, new()
//            where TPlayer1 : class, IPlayerAgent, new()
//        {
//            var arbiter = new Arbiter();
//            var b0 = arbiter.NewModel<BoardModel, int, int>(8, 8);
//            var c0 = arbiter.NewAgent<BoardAgent, IBoardModel>(b0);

//            var m0 = arbiter.NewModel<Player, EColor>(EColor.White);
//            var m1 = arbiter.NewModel<Player, EColor>(EColor.Black);
//            var p0 = arbiter.NewAgent<TPlayer0, IPlayerModel>(m0);
//            var p1 = arbiter.NewAgent<TPlayer1, IPlayerModel>(m1);

//            var d0 = arbiter.NewModel<Deck, Guid, IPlayerModel>(Guid.Empty, m0);
//            var d1 = arbiter.NewModel<Deck, Guid, IPlayerModel>(Guid.Empty, m1);

//            m0.SetDeck(d0);
//            m1.SetDeck(d1);

//            arbiter.Setup(c0, p0, p1);

//            return arbiter;
//        }
//    }
//}
