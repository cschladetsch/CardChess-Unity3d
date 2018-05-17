using System;

namespace App.Model
{
    using Common;

    public class Deck :
        CardCollection,
        IDeck,
        ICreateWith<Guid, IOwner>
    {
        public override int MaxCards => Parameters.MinCardsInDeck;
        public bool Create(Guid a0, IOwner owner)
        {
            base.Create(owner);
            // TODO: use guid to find the deck from deck-builder list
            return true;
        }

        public void NewGame()
        {
            for (var n = 0; n < MaxCards; ++n)
            {
                var tmpl = Database.CardTemplates.GetRandom();
                var card = Arbiter.Instance.NewCardModel(tmpl, Owner);
                Add(card);
            }
        }

        public bool Create(ITemplateDeck a0)
        {
            throw new NotImplementedException();
        }

        public void Shuffle()
        {
            cards.Shuffle();
        }

        public ICard Draw()
        {
            if (Empty)
            {
                Warn("Empty");
                return null;
            }
            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public bool AddToBottom(ICard card)
        {
            if (Maxxed)
            {
                Warn("Maxxed");
                return false;
            }
            cards.Insert(cards.Count, card);
            return true;
        }

        public int ShuffleIn(params ICard[] cards)
        {
            var added = 0;
            foreach (var card in cards)
            {
                if (Maxxed)
                    return added;
                ShuffleIn(card);
                ++added;
            }
            return added;
        }
    }
}
