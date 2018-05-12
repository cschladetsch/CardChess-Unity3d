using UnityEngine;
using NUnit.Framework;
using System.Collections;
using App;

using App.Agent;

namespace App
{
    [TestFixture]
    public class TestKingPlacement
    {

        [Test]
        public void TestKing()
        {

            var board = new Model.Board(8, 8);
            var arbiter = new Arbiter(board);

            var d0 = arbiter.NewModel<Model.Deck>();
            var d1 = arbiter.NewModel<Model.Deck>();

            var p0 = arbiter.NewModel<Model.Player, EColor, Model.IDeck>(EColor.White, d0);
            var p1 = arbiter.NewModel<Model.Player, EColor, Model.IDeck>(EColor.Black, d1);

            var a0 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(p0);
            var a1 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(p1);

            arbiter.SetPlayers(a0, a1);
            arbiter.NewGame();
            arbiter.StartGame();
        }
    }
}
