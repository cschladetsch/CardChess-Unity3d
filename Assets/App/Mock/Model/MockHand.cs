using App.Common;

namespace App.Mock.Model
{
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
