using System.Collections.Generic;

namespace App.Model
{
    public interface ICardCollection
    {
        IList<ICard> Cards { get; }
        void Add(ICard card);
        void Remove(ICard card);
    }
}