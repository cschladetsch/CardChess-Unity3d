using System;
using System.Collections.Generic;

namespace App.Model
{
    using Common;
    using ICard = Common.ICard;

    /// <summary>
    /// All cards owned by a playerAgent
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

        public bool Add(ICardModelTemplate cardModel)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ICardModelTemplate cardModel)
        {
            throw new NotImplementedException();
        }

        public bool Has(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICardModelTemplate Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Create(Guid id)
        {
            return true;
        }

        protected IList<ICardModelTemplate> cards = new List<ICardModelTemplate>();
    }
}
