using System.Collections.Generic;
using System.Linq;

namespace App.Agent
{
    using App.Model;

    public abstract class CardCollection :
        AgentBaseCoro<Model.ICardCollection>,
        ICardCollection
    {
        public int MaxCards => Model.MaxCards;
        public IEnumerable<Common.ICard> Cards => Model.Cards;
        public int NumCards => Cards.Count();
        public bool Empty => NumCards == 0;
        public bool Maxxed => NumCards == MaxCards;
        public IPlayerAgent PlayerAgent => Owner as IPlayerAgent;
        public IHandAgent Hand => PlayerAgent.Hand;
        public IDeckAgent Deck => PlayerAgent.Deck;

        public bool Add(Common.ICard card)
        {
            return Model.Add(card);
        }

        public bool Remove(Common.ICard card)
        {
            return Model.Remove(card);
        }
    }
}
