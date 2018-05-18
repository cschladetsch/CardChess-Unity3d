using System;
using System.Collections.Generic;

namespace App.Model
{
    /// <summary>
    /// Common to other collections of cards for Models, including DeckModel, HandModel and Graveyard.
    /// </summary>
    public abstract class CardCollectionModelBase :
        ModelBase,
        Common.ICardCollection<Model.ICardModel>
    {
        public abstract int MaxCards { get; }
        public IEnumerable<Common.ICard> Cards => cards;
        public int NumCards => cards.Count;
        public bool Empty => NumCards == 0;
        public bool Maxxed => cards.Count == MaxCards;
        public IPlayerModel PlayerModel => Owner as IPlayerModel;
        public IHandModel HandModel => PlayerModel.HandModel;
        public IDeckModel DeckModel => PlayerModel.DeckModel;

        public bool Add(ICardModel cardModel)
        {
            if (cards.Count == MaxCards)
                return false;
            cards.Add(cardModel);
            return true;
        }

        public bool Remove(ICardModel cardModel)
        {
            if (cards.Count == 0)
                return false;
            cards.Add(cardModel);
            return true;
        }

        protected List<ICardModel> cards = new List<ICardModel>();
    }
}
