using App.Common;
using App.Registry;
using App.Service;

namespace App.Model
{
    public class HandModel
        : CardCollectionModelBase
        , IHandModel
    {
        public override int MaxCards => Parameters.MaxCardsInHand;
        [Inject] public ICardTemplateService _cardTemplateService;

        public ICardModel this[int index]
        {
            get { return _Cards[index]; }
            set { _Cards[index] = value; }
        }

        public HandModel(IPlayerModel owner)
            : base(owner)
        {
        }

        public override void StartGame()
        {
            Info($"Hand StartGame {Player}");
            _Cards.Clear();
            DrawInitialCards();
        }

        public override void EndGame()
        {
            _Cards.Clear();
        }

        protected virtual void DrawInitialCards()
        {
            Deck.Shuffle();
            for (var n = 0; n < Parameters.StartHandCardCount; ++n)
            {
                var card = Deck.Draw();
                Assert.IsNotNull(card);
                Add(card);
            }

            AddKing();
        }

        protected void AddKing()
        {
            Info($"{Player} add King to hand, count={Cards.Count}");
            Add(_cardTemplateService.NewCardModel(Owner.Value as IPlayerModel, EPieceType.King));
        }
    }
}
