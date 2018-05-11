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
            var inj = new Injection();

            var board = new Model.Board(8, 8);
            var arbiter = new Arbiter(board);

            //var p0 = arbiter.NewModel<Model.Player>(EColor.White);

            //var p1 = new Model.Player(EColor.Black);


            //var a0 = arbiter.NewAgent<Player, Model.IPlayer>(p0);
            //var a1 = arbiter.NewAgent<Player, Model.IPlayer>(p1);

            //arbiter.SetPlayers(a0, a1);
            arbiter.NewGame();
            arbiter.StartGame();
        }
    }
}
