using System;

namespace App.Model
{
    public class CardCollectionDesc : IHasId
    {
        public Guid Id { get; }
        public string Name { get; }
    }
}
