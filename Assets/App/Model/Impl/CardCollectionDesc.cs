using System;

namespace App.Model
{
    public class CardCollectionDesc : IHasId
    {
        public string Name { get; }
        public Guid Id { get; }
    }
}
