using System;
using System.Collections.Generic;
using System.Linq;
using App.Common;
using UniRx;

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
        public IPlayerModel Player => Owner as IPlayerModel;
        public IHandModel Hand => Player.Hand;
        public IDeckModel Deck => Player.Deck;

        public IReadOnlyReactiveProperty<int> NumCards => _numCards;
        public IReadOnlyReactiveProperty<bool> Empty => _empty;
        public IReadOnlyReactiveProperty<bool> Maxxed => _maxxed;
        public IReadOnlyCollection<ICardModel> Cards => _Cards;
        #endregion

        #region Public Methods

        public override bool Construct(IOwner owner)
        {
            if (base.Construct(owner))
                return false;
            _numCards = new IntReactiveProperty(0);
            _empty = new BoolReactiveProperty(true);
            _maxxed = new BoolReactiveProperty(false);
            _cardList = new List<ICardModel>();
            _Cards = new ReactiveCollection<ICardModel>(_cardList);
            _Cards.ObserveCountChanged().Subscribe(
                n =>
                {
                    _numCards.Value = n;
                    _maxxed.Value = n == MaxCards;
                    _empty.Value = n == 0;
                }
            );

            return true;
        }

        public bool Has(ICardModel card)
        {
            return Has(card.Id);
        }

        public bool Has(Guid idCard)
        {
            return _Cards.Any(c => c.Id == idCard);
        }

        public bool Add(ICardModel cardModel)
        {
            if (Maxxed.Value)
                return false;
            _Cards.Add(cardModel);
            return true;
        }

        public void Add(IEnumerable<ICardModel> cards)
        {
            foreach (var card in cards)
                Add(card);
        }

        public bool Remove(ICardModel cardModel)
        {
            if (Empty.Value)
                return false;
            _Cards.Add(cardModel);
            return true;
        }

        #endregion

        protected UniRx.ReactiveCollection<ICardModel> _Cards;

        #region Private Fields

        private List<ICardModel> _cardList;
        private IntReactiveProperty _numCards;
        private BoolReactiveProperty _empty;
        private BoolReactiveProperty _maxxed;

        #endregion
    }
}
