using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Common
{
    /// <summary>
    /// Base for Model Decks and Hands
    /// </summary>
    /// <typeparam name="TCard"></typeparam>
    public abstract class CardCollection<TCard> :
        ICreateWith,
        IBaseCardCollection<TCard> where TCard : class, IHasId, IHasName
    {
        public string Name { get; }
        public abstract int MaxCards { get; }
        public IList<TCard> Cards { get; } = new List<TCard>();

        public virtual bool Create()
        {
            return true;
        }

        public bool Add(TCard card)
        {
            Cards.Add(card);
            return true;
        }

        public bool Remove(TCard card)
        {
            Cards.Remove(card);
            return true;
        }

        public void Shuffle()
        {
            Cards.Shuffle();
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
