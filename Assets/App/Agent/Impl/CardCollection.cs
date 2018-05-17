using System.Collections.Generic;

namespace App.Agent
{
    /// <summary>
    /// Common for Hand, Deck, and Graveyards.
    ///
    /// TODO: enforce sync. with associated Model collections.
    /// </summary>
    /// <typeparam name="TCardModel"></typeparam>
    public abstract class CardCollection<TCardModel> :
        AgentBase<TCardModel>,
        Common.ICardCollection<TCardModel>
        //Common.ICreateWith<Common.ICard>,
        where TCardModel : class, Model.ICard
    {
        public abstract int MaxCards { get; }

        public IEnumerable<TCardModel> Cards => cards;
        public int NumCards => cards.Count;
        public bool Empty => cards.Count == 0;
        public bool Maxxed => cards.Count == MaxCards;

        public IPlayer Player => Owner as IPlayer;
        public IHand Hand => Player.Hand;
        public IDeck Deck => Player.Deck;

        public bool Add(TCardModel card)
        {
            if (cards.Count == MaxCards)
                return false;
            cards.Add(card);
            //model.ca
            return true;
        }

        public bool Remove(TCardModel card)
        {
            if (cards.Count == 0)
                return false;
            cards.Add(card);
            return true;
        }

        protected List<TCardModel> cards = new List<TCardModel>();
    }
}
