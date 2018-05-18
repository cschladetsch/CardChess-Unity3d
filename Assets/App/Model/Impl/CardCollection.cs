using System;
using System.Collections.Generic;

namespace App.Model
{
    /// <summary>
    /// Common to other collections of cards for Models, including Deck, Hand and Graveyard.
    /// </summary>
    public abstract class CardCollection :
        ModelBase,
        Common.ICardCollection<Model.ICard>
    {
        public abstract int MaxCards { get; }
        public IEnumerable<Common.ICard> Cards => cards;
        public int NumCards => cards.Count;
        public bool Empty => NumCards == 0;
        public bool Maxxed => cards.Count == MaxCards;
        public IPlayer Player => Owner as IPlayer;
        public IHand Hand => Player.Hand;
        public IDeck Deck => Player.Deck;

        public bool Add(ICard card)
        {
            if (cards.Count == MaxCards)
                return false;
            cards.Add(card);
            return true;
        }

        public bool Remove(ICard card)
        {
            if (cards.Count == 0)
                return false;
            cards.Add(card);
            return true;
        }

        protected List<ICard> cards = new List<ICard>();
    }
}
