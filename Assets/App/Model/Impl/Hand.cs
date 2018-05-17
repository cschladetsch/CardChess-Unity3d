using System.Collections.Generic;

namespace App.Model
{
    public class Hand :
        CardCollection,
        IHand
    {
        public override int MaxCards => Parameters.MaxCardsInHand;

        public void NewGame()
        {
            cards = new List<ICard>();
            for (var n = 0; n < Parameters.StartHandCardCount; ++n)
            {
                Add(Deck.Draw());
            }
        }
    }
}
