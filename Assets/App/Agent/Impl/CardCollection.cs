using System.Collections.Generic;
using System.Linq;

namespace App.Agent
{
    using App.Model;

    public class CardCollection :
        AgentBase<Model.ICardCollection>
    {
        public int MaxCards => Model.MaxCards;
        public IEnumerable<Common.ICard> Cards => Model.Cards;
        public int NumCards => Cards.Count();
        public bool Empty => NumCards == 0;
        public bool Maxxed => NumCards == MaxCards;
        public IPlayer Player => Owner as IPlayer;
        public IHand Hand => Player.Hand;
        public IDeck Deck => Player.Deck;
    }
}
