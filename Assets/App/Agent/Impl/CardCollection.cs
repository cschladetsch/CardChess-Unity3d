using System.Collections.Generic;
using System.Linq;

namespace App.Agent
{
	using App.Model;

    /// <summary>
    /// Common for Hand, Deck, and Graveyards.
    ///
    /// TODO: enforce sync. with associated Model collections.
    /// </summary>
	public class CardCollection :
        AgentBase<Model.ICardCollection>
	{
		public int MaxCards => Model.MaxCards;

		public IEnumerable<ICard> Cards => Model.Cards;
        public int NumCards => Cards.Count();
		public bool Empty => NumCards == 0;
		public bool Maxxed => NumCards == MaxCards;

        public IPlayer Player => Owner as IPlayer;
        public IHand Hand => Player.Hand;
        public IDeck Deck => Player.Deck;
    }
}