using App.Common;

namespace App.Mock.Model
{
    using App.Model;

    public class MockHand
        : HandModel
    {
        public MockHand(IPlayerModel owner)
            : base(owner)
        {
        }
    }
}
