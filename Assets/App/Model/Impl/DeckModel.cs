using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    public class DeckModel
        : CardCollectionModelBase
        , IDeckModel
    {
        public override int MaxCards => Parameters.MinCardsInDeck;

        public DeckModel(ITemplateDeck templateDeck, IOwner owner)
            : base(owner)
        {
            // TODO: store and use templateDeck
        }

        public virtual void NewGame()
        {
            _Cards.Clear();
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

        public virtual void Shuffle()
        {
            _Cards.Shuffle();
        }

        public ICardModel Draw()
        {
            if (Empty.Value)
            {
                Warn("Empty Deck");
                Player.CardExhaustion();
                return null;
            }
            var card = _Cards[0];
            _Cards.RemoveAt(0);
            return card;
        }

        public IEnumerable<ICardModel> Draw(int count)
        {
            foreach (var card in _Cards.Take(count))
            {
                if (card == null)
                {
                    Player.CardExhaustion();
                    yield break;
                }
                _Cards.Remove(card);
                yield return card;
            }
        }

        public bool AddToBottom(ICardModel cardModel)
        {
            if (Maxxed.Value)
            {
                Warn("Maxxed");
                return false;
            }
            _Cards.Insert(_Cards.Count, cardModel);
            return true;
        }

        public virtual int ShuffleIn(params ICardModel[] models)
        {
            var added = 0;
            foreach (var card in models)
            {
                if (Maxxed.Value)
                    return added;
                ShuffleIn(card);
                ++added;
            }
            return added;
        }
    }
}
