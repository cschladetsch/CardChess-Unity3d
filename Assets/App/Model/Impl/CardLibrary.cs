using System;
using System.Collections.Generic;

namespace App.Model
{
    using Common;
    using ICard = Common.ICard;

    /// <summary>
    /// All cards owned by a player
    /// </summary>
    public abstract class CardLibrary :
        ModelBase,
        ICardCollection<Common.ICard>,
        ICreateWith<Guid>
    {
        public int MaxCards => int.MaxValue;

        public IEnumerable<Common.ICard> Cards => cards;
        public int NumCards { get; }
        public bool Empty => cards.Count == 0;
        public bool Maxxed => cards.Count == MaxCards;

        public bool Add(Common.ICard card)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Common.ICard card)
        {
            throw new NotImplementedException();
        }

        public bool Add(ICardTemplate card)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ICardTemplate card)
        {
            throw new NotImplementedException();
        }

        public bool Has(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICardTemplate Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Create(Guid id)
        {
            return true;
        }

        protected IList<ICardTemplate> cards = new List<ICardTemplate>();
    }
}
