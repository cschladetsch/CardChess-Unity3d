using System.Diagnostics;
using App.Model;
using NUnit.Framework;

using UniRx;

namespace App.Agent.Test
{
    [TestFixture]
    class TestAgent : TestAgentBase
    {
        [Test]
        public void TestReactiveCollection()
        {
            var _nums = new ReactiveCollection<int>();
            IReadOnlyReactiveCollection<int> nums = _nums;
            nums.ObserveAdd().Subscribe(Add);
            _nums.Add(42);

            Assert.AreEqual(0, index);
            Assert.AreEqual(42, added);
        }

        private int index = -1;
        private int added = -1;

        private void Add(CollectionAddEvent<int> add)
        {
            index = add.Index;
            added = add.Value;
        }

        [Test]
        public void TestString()
        {
            double d = 12.3456;
            Trace.WriteLine($"{d,3}");
        }

        [Test]
        public void TestBasicGameAgents()
        {
            _arbiterAgent.PrepareGame(_whiteAgent, _blackAgent);
            _arbiterAgent.StartGame();

            for (int n = 0; n < 100; ++n)
            {
                _arbiterAgent.Step();
                //Info($"{_arbiterAgent.Kernel.Root}");
                Info(_board.Print());
                if (_arbiter.GameState.Value == EGameState.Completed)
                    break;
            }
        }
    }
}
