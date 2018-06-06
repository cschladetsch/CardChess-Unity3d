using System;
using System.Collections.Generic;
using UniRx;

namespace App.Common
{
    public interface ICardCollectionBase
        : IOwned
    {
    }

    /// <summary>
    /// A generic collection of cards
    /// </summary>
    public interface ICardCollection<TCard>
        : ICardCollectionBase
        where TCard : class
    {
        event Action<ICardCollectionBase> Overflow;
        int MaxCards { get; }
        IReadOnlyReactiveProperty<int> NumCards { get; }
        IReadOnlyReactiveProperty<bool> Empty { get; }
        IReadOnlyReactiveProperty<bool> Maxxed { get; }
        IReadOnlyReactiveCollection<TCard> Cards { get; }

        void Clear();
        bool Has(TCard card);
        bool Has(Guid idCard);
        bool Add(TCard card);
        int Add(IEnumerable<TCard> cards);
        bool Remove(TCard card);
        void Shuffle();
        int ShuffleIn(IEnumerable<TCard> cards);
        bool ShuffleIn(TCard card);
        bool AddToBottom(TCard cardModel);
    }
}
