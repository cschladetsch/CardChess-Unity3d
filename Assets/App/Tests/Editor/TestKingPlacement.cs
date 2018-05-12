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

            var board = new Model.Board();
            var arbiter = new Arbiter();

            var d0 = arbiter.NewModel<Model.Deck>();
            var d1 = arbiter.NewModel<Model.Deck>();

            var p0 = arbiter.NewModel<Model.Player, EColor, Model.IDeck>(EColor.White, d0);
            var p1 = arbiter.NewModel<Model.Player, EColor, Model.IDeck>(EColor.Black, d1);

            var a0 = Arbiter.NewAgent<Agent.Player, Model.IPlayer>(p0);
            var a1 = Arbiter.NewAgent<Agent.Player, Model.IPlayer>(p1);

            //var a3 = arbiter.NewEntity<Model.IPlayer, EColor, Model.IDeck, Agent.Player>(EColor.White, d0);

            arbiter.SetPlayers(a0, a1);
            arbiter.NewGame();
            arbiter.StartGame();
        }
    }
}
