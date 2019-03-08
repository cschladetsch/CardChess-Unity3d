using System.Collections.Generic;

namespace App.Model
{
    public class DeckModel
        : CardCollectionModelBase
        , IDeckModel
    {
        public override int MaxCards => Parameters.MinCardsInDeck;

        public DeckModel(ITemplateDeck templateDeck, IPlayerModel owner)
            : base(owner)
        {
            // TODO: store and use templateDeck
        }

        public override void StartGame()
        {
            _Cards.Clear();
            for (var n = 0; n < MaxCards; ++n)
            {
                // LATER: use a pre-made deck (CardLibrary)
                var tmpl = Database.CardTemplates.GetRandom();
                var card = Registry.New<ICardModel>(Owner.Value, tmpl);
                Add(card);
            }
        }

        public override void EndGame()
        {
            _Cards.Clear();
        }

        public ICardModel Draw()
        {
            if (Empty.Value)
            {
                Warn($"Empty Deck {this}");
                Player.CardExhaustion();
                return null;
            }

            var card = _Cards[0];
            _Cards.RemoveAt(0);
            return card;
        }

        public IEnumerable<ICardModel> Draw(int count)
        {
            for (var n = 0; n < count; ++n)
            {
                yield return Draw();
            }
        }
    }
}
