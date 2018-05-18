using System;
using System.Collections.Generic;

namespace App.Common
{
    public interface ICardCollection<TCard>
        where TCard : class, ICard
    {
        int MaxCards { get; }
        IEnumerable<ICard> Cards { get; }
        int NumCards { get; }
        bool Empty { get; }
        bool Maxxed { get; }

        bool Add(TCard card);
        bool Remove(TCard card);
    }
}
