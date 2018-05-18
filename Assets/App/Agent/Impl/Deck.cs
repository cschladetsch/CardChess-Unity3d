using System.Collections.Generic;
using System.Linq;

namespace App.Agent
{
    using ICardModel = Model.ICard;

    public class Deck :
        CardCollection//<ICardModel>
    {
        public new int MaxCards => 50;

        public IEnumerable<ICardModel> DrawCards(uint n)
        {
            while (n-- > 0)
            {
                yield return DrawTopCard();
            }
        }

        public ICardModel DrawTopCard()
        {
            return null;
        }
    }
}
