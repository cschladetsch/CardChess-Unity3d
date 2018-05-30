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

        public Response NewGame()
        {
            var count = Parameters.StartHandCardCount;
            _Cards.Clear();
            if (Deck.NumCards.Value < count)
            {
                Error($"Need more cards in {Deck}");
                return Response.Fail;
            }
            Add(Deck.Draw(count));
            return Response.Ok;
        }
    }
}
