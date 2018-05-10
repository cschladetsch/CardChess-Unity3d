using System.Collections.Generic;

namespace App.Model
{
    public interface ICardCollection
    {
        int MaxCards { get; }
        IList<ICard> Cards { get; }

        void Add(ICard card);
        void Remove(ICard card);
    }
}
