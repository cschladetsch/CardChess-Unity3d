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
            _arbiterAgent.StartGame(_whiteAgent, _blackAgent);
            Trace.WriteLine(_arbiterAgent.Kernel.Root);

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());

            _arbiterAgent.Step();
            Trace.WriteLine(_arbiterAgent.Kernel.Root);
            Trace.WriteLine(_board.Print());
        }
    }
}
