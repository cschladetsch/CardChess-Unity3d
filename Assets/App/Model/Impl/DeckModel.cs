using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    public class DeckModel :
        CardCollectionModelBase,
        IDeckModel
    {
        public override int MaxCards => Parameters.MinCardsInDeck;

        public DeckModel(Guid a0, IOwner owner)
            : base(owner)
        {
        }

        public void NewGame()
        {
            cards.Clear();
            for (var n = 0; n < MaxCards; ++n)
            {
                // LATER: use a pre-made deck (CardLibrary)
                var tmpl = Database.CardTemplates.GetRandom();
                var card = Registry.New<ICardModel>(tmpl, Owner);
                Add(card);
            }
        }

        public bool Construct(ITemplateDeck a0)
        {
            throw new NotImplementedException();
        }

        public void Shuffle()
        {
            cards.Shuffle();
        }

        public ICardModel Draw()
        {
            if (Empty)
            {
                Warn("Empty Deck");
                Player.CardExhaustion();
                return null;
            }
            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public IEnumerable<ICardModel> Draw(int count)
        {
            foreach (var card in cards.Take(count))
            {
                if (card == null)
                {
                    Player.CardExhaustion();
                    yield break;
                }
                cards.Remove(card);
                yield return card;
            }
        }

        public bool AddToBottom(ICardModel cardModel)
        {
            if (Maxxed)
            {
                Warn("Maxxed");
                return false;
            }
            cards.Insert(cards.Count, cardModel);
            return true;
        }

        public int ShuffleIn(params ICardModel[] cardsModel)
        {
            var added = 0;
            foreach (var card in cardsModel)
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
