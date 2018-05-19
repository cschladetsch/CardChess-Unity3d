using System;
using System.Collections.Generic;

namespace App.Common
{
    public interface ICardCollectionBase
        : IOwned
    {
    }

    public interface ICardCollection<TCard>
        : ICardCollectionBase
        where TCard : class, ICard
    {
        int MaxCards { get; }
        int NumCards { get; }
        bool Empty { get; }
        bool Maxxed { get; }

        IEnumerable<ICard> Cards { get; }
        bool Has(TCard card);
        bool Has(Guid idCard);
        bool Add(TCard card);
        bool Remove(TCard card);
    }
}
