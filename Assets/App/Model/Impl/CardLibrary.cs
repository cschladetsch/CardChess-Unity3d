using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;
    using ICard = Common.ICard;

    /// <summary>
    /// All cards owned by a playerAgent
    /// </summary>
    public abstract class CardLibrary :
        ModelBase,
        ICardCollection<ICard>,
        IConstructWith<Guid>
    {
        public int MaxCards => int.MaxValue;
        public IEnumerable<Common.ICard> Cards => cards;
        public int NumCards => cards.Count;
        public bool Empty => NumCards == 0;
        public bool Maxxed => NumCards == MaxCards;

        public bool Construct(Guid id)
        {
            return true;
        }

        public bool Add(ICard card)
        {
            Assert.IsNotNull(card);
            var templ = card as ICardTemplate;
            if (templ == null)
            {
                Warn($"{card} is of type {card.GetType()}, but should be a {typeof(ICardTemplate)}");
                return false;
            }
            cards.Add(templ);
            return true;
        }

        public void Add(IEnumerable<ICard> cads)
        {
            foreach (var card in cards)
            {
                if (card == null)
                {
                    Warn($"Attempt to add null card to {this}");
                    continue;
                }
                Add(card);
            }
        }

        public bool Remove(ICard card)
        {
            var templ = card as ICardTemplate;
            if (templ == null)
            {
                Warn($"{card} is of type {card.GetType()}, but should be a {typeof(ICardTemplate)}");
                return false;
            }
            if (!Has(card))
                return false;
            cards.Remove(templ);
            return true;
        }

        public bool Add(ICardTemplate card)
        {
            if (Maxxed)
                return false;
            cards.Add(card);
            return true;
        }

        public bool Remove(ICardTemplate card)
        {
            return Has(card) && cards.Remove(card);
        }

        public bool Has(ICard card)
        {
            return Has(card.Id);
        }

        public bool Has(Guid id)
        {
            return cards.Any(c => c.Id == id);
        }

        public ICardTemplate Get(Guid id)
        {
            return cards.First(c => c.Id == id);
        }

        protected IList<ICardTemplate> cards = new List<ICardTemplate>();
    }
}
