using System.Collections.Generic;

public interface ICardCollection
{
    IList<ICard> Cards { get; }
    void Add(ICard card);
    void Remove(ICard card);
}