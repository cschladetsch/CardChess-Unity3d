using System.Collections.Generic;
using App.Model;
using CoLib;
using UnityEngine;

using UniRx;

namespace App.View.Impl1
{
    using Agent;
    using Common;

    public class HandView
        : ViewBase<IHandAgent>
        , IHandView
    {
        public CardView CardViewPrefab;
        public Transform CardsRoot;
        public int MockNumCards = 4;
        public Vector3 Offset;

        private ReactiveProperty<ICardView> CurrentHover = new ReactiveProperty<ICardView>();

        public override void SetAgent(IHandAgent hand)
        {
            base.SetAgent(hand);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            Clear();
            CreateHandView();

            // TODO: hand.Cards, not hand.Model.Cards
            hand.Model.Cards.ObserveAdd().Subscribe(Add);
            hand.Model.Cards.ObserveRemove().Subscribe(Remove);

            _cards.ObserveCountChanged().Subscribe(_ => Redraw());
        }

        void Redraw()
        {
            CreateHandView();
        }

        void Add(CollectionAddEvent<ICardModel> add)
        {
            _cards.Insert(add.Index, Registry.New<ICardView>(add.Value));
        }

        void Remove(CollectionRemoveEvent<ICardModel> card)
        {
            var view = _cards[card.Index];
            view.Destroy();
            _cards.RemoveAt(card.Index);
        }

        public void CreateHandView()
        {
            Clear();

            var model = Agent.Model;
            var n = 0;
            foreach (var card in model.Cards)
            {
                var view = Instantiate(CardViewPrefab) as ICardView;
                var tr = view.GameObject.transform;
                tr.SetParent(CardsRoot);
                tr.localPosition = n * Offset;
                view.SetAgent(Agent.Registry.New<ICardAgent>(card));
                view.GameObject.name = $"{card}";
                _cards.Add(view);
                ++n;
            }
        }

        private IReactiveProperty<ICardView> _scaled;

        private CommandDelegate ScaleTo(ICardView card, float scale)
        {
            return Commands.ScaleTo(
                    card.GameObject,
                    scale,
                    1.0
            );
        }

        [ContextMenu("HandView-Clear")]
        public void Clear()
        {
            foreach (Transform tr in CardsRoot.transform)
                Unity.Destroy(tr);
        }

        private ReactiveCollection<ICardView> _cards = new ReactiveCollection<ICardView>();
    }
}
