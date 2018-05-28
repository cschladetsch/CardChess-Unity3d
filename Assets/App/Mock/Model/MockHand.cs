using System;

namespace App.Mock.Model
{
    using Common;
    using App.Model;

    public class MockHand
        : HandModel
    {
        public MockHand(IOwner owner)
            : base(owner)
        {
        }
    }
}
