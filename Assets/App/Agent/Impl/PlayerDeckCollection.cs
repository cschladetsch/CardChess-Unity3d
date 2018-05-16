using System.Collections.Generic;
using System.Linq;
using App.Model;

namespace App.Agent
{
    using Common;

    public class PlayerDeckCollection :
        CardCollection<ICardInstance>
    {
        public override int MaxCards => 50;

        public IEnumerable<ICardInstance> DrawCards(uint n)
        {
            while (n-- > 0)
            {
                yield return DrawTopCard();
            }
        }

        public ICardInstance DrawTopCard()
        {
            if (Cards.Count == 0)
            {
                //Warn($"{Name}: No cards to draw from deck");
                return null;
            }
            var card = Cards.First();
            Cards.RemoveAt(0);
            return card;
        }
    }
}
