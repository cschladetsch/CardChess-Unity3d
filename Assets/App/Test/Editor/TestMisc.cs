using System.Diagnostics;
using NUnit.Framework;
using UniRx;

namespace App.Agent.Test
{
    // just things I wanted to test for sanity testing
    [TestFixture]
    class TestMisc
    {
        [Test]
        public void TestReactiveCollection()
        {
            var _nums = new ReactiveCollection<int>();
            IReadOnlyReactiveCollection<int> nums = _nums;
            nums.ObserveAdd().Subscribe(Add);
            _nums.Add(42);

            Assert.AreEqual(0, _index);
            Assert.AreEqual(42, _added);
        }

        private int _index = -1;
        private int _added = -1;

        private void Add(CollectionAddEvent<int> add)
        {
            _index = add.Index;
            _added = add.Value;
        }

        [Test]
        public void TestString()
        {
            double d = 12.3456;
            Trace.WriteLine($"{d,3}");
        }
    }
}
