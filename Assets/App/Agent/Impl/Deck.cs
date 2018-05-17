using System.Collections.Generic;
using System.Linq;

namespace App.Agent
{
    using ICardModel = Model.ICard;

    public class Deck :
        CardCollection<ICardModel>
    {
        public override int MaxCards => 50;

        public IEnumerable<ICardModel> DrawCards(uint n)
        {
            while (n-- > 0)
            {
                yield return DrawTopCard();
            }
        }

        public ICardModel DrawTopCard()
        {
            if (Empty)
            {
                //Warn($"{Name}: No cards to draw from deck");
                return null;
            }
            var card = Cards.First();
            cards.RemoveAt(0);
            return card;
        }
    }
}
