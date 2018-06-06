namespace App.Model
{
    using Common.Message;

    public class HandModel
        : CardCollectionModelBase
        , IHandModel
    {
        public override int MaxCards => Parameters.MaxCardsInHand;

        public ICardModel this[int index]
        {
            get { return _Cards[index]; }
            set { _Cards[index] = value; }
        }

        public HandModel(IPlayerModel owner)
            : base(owner)
        {
        }

        public override void Prepare()
        {
        }

        public void NewGame()
        {
            var count = Parameters.StartHandCardCount;
            _Cards.Clear();
            if (Deck.NumCards.Value < count)
            {
                Error($"Need more cards in {Deck}");
                return;
            }
            DrawInitialCards(count);
        }

        protected virtual void DrawInitialCards(int count)
        {
            Add(Deck.Draw(count));
        }

        public void EndGame()
        {
        }
    }
}
