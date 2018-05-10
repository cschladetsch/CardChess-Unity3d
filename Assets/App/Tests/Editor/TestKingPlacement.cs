using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using App;

using App.Agent;

namespace App
{
    public class TestKingPlacement
    {

        [Test]
        public void TestKing()
        {
            var board = new Model.Board(8, 8);
            var p0 = new Model.Player(EColor.White);
            var p1 = new Model.Player(EColor.Black);

            var arbiter = new Arbiter(board);

            var ap0 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(p0);
            var ap1 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(p1);

            arbiter.NewGame();
            arbiter.StartGame();
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}
