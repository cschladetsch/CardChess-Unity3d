using App.Common;
using App.Service;
using Dekuple;

namespace App.Model
{
    public class HandModel
        : CardCollectionModelBase
        , IHandModel
    {
        public override int MaxCards => Parameters.MaxCardsInHand;
        public ICardModel this[int index] { get => _Cards[index]; set => _Cards[index] = value; }

        [Inject] public ICardTemplateService _cardTemplateService;

        public HandModel(IPlayerModel owner)
            : base(owner)
        {
        }

        public override void StartGame()
        {
            Verbose(20, $"Hand StartGame {Player}");
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
            Add(_cardTemplateService.NewCardModel(Owner.Value as IPlayerModel, EPieceType.King));
        }
    }
}
