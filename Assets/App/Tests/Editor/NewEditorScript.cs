using System;

namespace App.Model
{
    using Common;
    
    public class MockDeck : DeckModel
    {
        public MockDeck(Guid a0, IOwner owner)
            : base(a0, owner)
        {
        }
    }
}
