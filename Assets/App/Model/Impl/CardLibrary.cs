using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    /// <summary>
    /// All cards owned by a playerAgent
    /// </summary>
    public class CardLibrary
        : ModelBase
    {
        public int MaxCards => int.MaxValue;
        public List<ICardTemplate> Cards = new List<ICardTemplate>();
        public int NumCards => _cards.Count;
        public bool Empty => NumCards == 0;
        public bool Maxxed => NumCards == MaxCards;

        protected CardLibrary(IOwner owner, Guid id)
            : base(owner)
        {
        }

        public bool Add(ICardTemplate card)
        {
            Assert.IsNotNull(card);
            if (_cards.Count(c => c.Id == card.Id) > 2)
            {
                Warn($"Cannot have more than three instances of {card} in your library");
                return false;
            }
            _cards.Add(card);
            return true;
        }

        public void Add(IEnumerable<ICardTemplate> cards)
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
            _cards.Remove(templ);
            return true;
        }

        public bool Has(ICardTemplate card)
        {
            return Has(card.Id);
        }

        public bool Has(Guid id)
        {
            return _cards.Any(c => c.Id == id);
        }

        public ICardTemplate Get(Guid id)
        {
            return _cards.First(c => c.Id == id);
        }

        protected IList<ICardTemplate> _cards = new List<ICardTemplate>();
    }
}
