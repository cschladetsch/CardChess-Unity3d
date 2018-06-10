using System.Diagnostics;
using NUnit.Framework;

namespace App.Agent.Test
{
    [TestFixture]
    class TestAgent : TestAgentBase
    {
        [Test]
        public void TestBoardAgentCreation()
        {

        }

        [Test]
        public void TestBasicAgentTurns()
        {
            _arbiterAgent.PrepareGame(_whiteAgent, _blackAgent);
            _arbiterAgent.StartGame();

            for (int n = 0; n < 8; ++n)
            {
                _arbiterAgent.Step();
                Info($"{_arbiterAgent.Kernel.Root}");
                Info(_board.Print());
            }
        }
    }
}
