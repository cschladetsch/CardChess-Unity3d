using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    /// <summary>
    /// All cards owned by a playerAgent
    /// </summary>
    public abstract class CardLibrary :
        ModelBase,
        IConstructWith<Guid>
    {
        public int MaxCards => int.MaxValue;
        public List<ICardTemplate> Cards = new List<ICardTemplate>();
        public int NumCards => cards.Count;
        public bool Empty => NumCards == 0;
        public bool Maxxed => NumCards == MaxCards;

        public bool Construct(Guid id)
        {
            return true;
        }

        public bool Add(ICardTemplate card)
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

        public void Add(IEnumerable<ICardTemplate> cads)
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

        public bool Remove(ICardTemplate card)
        {
            var templ = card;
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

        public bool Has(ICardTemplate card)
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
