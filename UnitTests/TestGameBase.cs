using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class TestGameBase
    {
        /// <summary>
        /// Make a basic setup of a board with two players.
        /// Each player has a randomly generated Deck and a Hand.
        /// </summary>
        /// <typeparam name="TPlayer0">Agent for White player</typeparam>
        /// <typeparam name="TPlayer1">Agent for Black player</typeparam>
        /// <returns></returns>
        protected static Arbiter RandomBasicSetup<TPlayer0, TPlayer1>()
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

        protected static void StepArbiter(uint n = 1)
        {
            while (n-- > 0)
                Arbiter.Instance.Step();
        }

        protected static void Step(Flow.IGenerator gen, uint steps)
        {
            while (steps-- > 0)
                gen.Step();
        }
    }
}
