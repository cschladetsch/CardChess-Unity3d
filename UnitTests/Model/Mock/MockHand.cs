using System;

namespace App.Model.Test
{
    using Common;
    using Model;

    public class MockHand
        : HandModel
    {
        public MockHand(IOwner owner)
            : base(owner)
        {
        }
    }
}
