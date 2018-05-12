using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    public abstract class CardCollection<TCard> : ModelBase, ICardCollection<TCard> where TCard: IHasId
    {
        public abstract int MaxCards { get; }

        public IList<TCard> Cards { get; } = new List<TCard>();

        public bool Add(TCard card)
        {
            if (Cards.Count >= MaxCards)
                return false;
            Cards.Add(card);
            return true;
        }

        public bool Remove(TCard card)
        {
            if (!Cards.Contains(card))
                return false;
            Cards.Remove(card);
            return true;
        }

        public bool Has(Guid id)
        {
            return Get(id) != null;
        }

        public TCard Get(Guid id)
        {
            return Cards.FirstOrDefault(c => c.Id == id);
        }
    }
}
