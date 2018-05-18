using System.Collections.Generic;
using App.Common;

namespace App.Model
{
    public class HandModel :
        CardCollectionModelBase,
        IHandModel
    {
        public override int MaxCards => Parameters.MaxCardsInHand;

        public Response NewGame()
        {
            var count = Parameters.StartHandCardCount;
            cards = new List<ICardModel>();
            if (DeckModel.NumCards < count)
            {
                Error("Need more cards in DeckModel");
                return new Response(EResponse.Fail);
            }
            while (count-- > 0)
            {
                Add((ICardModel)DeckModel.Draw());
            }
            return Response.Ok;
        }
    }
}
