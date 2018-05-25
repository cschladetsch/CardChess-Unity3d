using System.Collections.Generic;
using App.Common;

namespace App.Model
{
    public class HandModel
        : CardCollectionModelBase
        , IHandModel
    {
        public override int MaxCards => Parameters.MaxCardsInHand;

        public HandModel(IOwner owner)
            : base(owner)
        {
        }

        public Response NewGame()
        {
            var count = Parameters.StartHandCardCount;
            cards = new List<ICardModel>();
            if (Deck.NumCards < count)
            {
                Error("Need more cards in Deck");
                return new Response(EResponse.Fail);
            }
            Add(Deck.Draw(count));
            return Response.Ok;
        }
    }
}
