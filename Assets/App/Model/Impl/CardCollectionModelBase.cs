using System;
using System.Collections.Generic;
using System.Linq;
using App.Common;

namespace App.Model
{
    /// <summary>
    /// Common to other collections of cards for Models, including Deck, Hand and Graveyard.
    /// </summary>
    public abstract class CardCollectionModelBase
        : ModelBase
        , ICardCollection<ICardModel>
    {
        #region Public Properties
        public abstract int MaxCards { get; }
        public IEnumerable<ICardModel> Cards => cards;
        public int NumCards => cards.Count;
        public bool Empty => NumCards == 0;
        public bool Maxxed => cards.Count == MaxCards;
        public IPlayerModel Player => Owner as IPlayerModel;
        public IHandModel Hand => Player.Hand;
        public IDeckModel Deck => Player.Deck;
        #endregion

        #region Public Methods
        public bool Has(ICardModel card)
        {
            return Has(card.Id);
        }

        public bool Has(Guid idCard)
        {
            return cards.Any(c => c.Id == idCard);
        }

        public bool Add(ICardModel cardModel)
        {
            if (cards.Count == MaxCards)
                return false;
            cards.Add(cardModel);
            return true;
        }

        public void Add(IEnumerable<ICardModel> cards)
        {
            foreach (var card in cards)
                Add(card);
        }

        public bool Remove(ICardModel cardModel)
        {
            if (cards.Count == 0)
                return false;
            cards.Add(cardModel);
            return true;
        }

        #endregion

        #region Protected
        protected CardCollectionModelBase(IOwner owner)
        {
            Construct(owner);
        }

        protected List<ICardModel> cards = new List<ICardModel>();
        #endregion
    }
}
