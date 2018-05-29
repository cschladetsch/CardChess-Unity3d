using System;
using System.Collections.Generic;
using UniRx;

namespace App.Common
{
    public interface ICardCollectionBase
        : IOwned
    {
    }

    public interface ICardCollection<TCard>
        : ICardCollectionBase
        where TCard : class
    {
        int MaxCards { get; }
        IReadOnlyReactiveProperty<int> NumCards { get; }
        IReadOnlyReactiveProperty<bool> Empty { get; }
        IReadOnlyReactiveProperty<bool> Maxxed { get; }
        IReadOnlyCollection<TCard> Cards { get; }

        bool Has(TCard card);
        bool Has(Guid idCard);
        bool Add(TCard card);
        void Add(IEnumerable<TCard> cards);
        bool Remove(TCard card);
    }
}
