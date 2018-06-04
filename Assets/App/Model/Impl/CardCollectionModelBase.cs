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
        public event Action<ICardCollectionBase> Overflow;

        #region Public Properties
        public abstract int MaxCards { get; }
        public IPlayerModel Player => Owner.Value as IPlayerModel;
        public IHandModel Hand => Player.Hand;
        public IDeckModel Deck => Player.Deck;

        public IReadOnlyReactiveProperty<int> NumCards => _numCards;
        public IReadOnlyReactiveProperty<bool> Empty => _empty;
        public IReadOnlyReactiveProperty<bool> Maxxed => _maxxed;
        public IReadOnlyReactiveCollection<ICardModel> Cards => _Cards;
        #endregion

        #region Public Methods
        protected CardCollectionModelBase(IPlayerModel owner)
            : base(owner)
        {
            SetOwner(owner);
            _numCards = new IntReactiveProperty(0);
            _empty = new BoolReactiveProperty(true);
            _maxxed = new BoolReactiveProperty(false);
            _Cards = new ReactiveCollection<ICardModel>();
            _Cards.ObserveCountChanged().Subscribe(n =>
                {
                    _numCards.Value = n;
                    _maxxed.Value = n == MaxCards;
                    _empty.Value = n == 0;
                }
            );
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
            {
                Overflow?.Invoke(this);
                return false;
            }
            _Cards.Add(cardModel);
            return true;
        }

        public int Add(IEnumerable<ICardModel> cards)
        {
            var n = 0;
            foreach (var card in cards)
            {
                if (!Add(card))
                    return n;
                ++n;
            }
            return n;
        }

        public bool Remove(ICardModel card)
        {
            Assert.IsNotNull(card);
            if (Has(card.Id) && !Empty.Value)
                return _Cards.Remove(card);
            Warn($"Attempt to remove {card} that doesn't exist in {this}");
            return false;
        }

        public virtual void Shuffle()
        {
            _Cards.Shuffle();
        }

        public bool ShuffleIn(ICardModel card)
        {
            if (Maxxed.Value)
                return false;
            var index = Math.RandomRanged(0, _Cards.Count);
            _Cards.Insert(index, card);
            return true;
        }

        public int ShuffleIn(IEnumerable<ICardModel> models)
        {
            var n = 0;
            foreach (var card in models)
            {
                if (!ShuffleIn(card))
                    return n;
                ++n;
            }
            return n;
        }


        public bool AddToBottom(ICardModel card)
        {
            if (Maxxed.Value)
                return false;
            _Cards.Insert(_Cards.Count, card);
            return true;
        }

        #endregion

        protected UniRx.ReactiveCollection<ICardModel> _Cards;

        #region Private Fields
        private readonly IntReactiveProperty _numCards;
        private readonly BoolReactiveProperty _empty;
        private readonly BoolReactiveProperty _maxxed;
        #endregion
    }
}
