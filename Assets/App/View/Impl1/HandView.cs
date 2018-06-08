using System;
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
        public IReactiveProperty<ICardAgent> Hover => _hover;
        public Vector3 Offset;

        public override void SetAgent(IHandAgent hand)
        {
            base.SetAgent(hand);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            _color = hand.Owner.Value.Color;

            _hovered
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromSeconds(0.05))
                .Subscribe(sq => _hover.Value = sq);
            Hover.Subscribe(sq =>
            {
                if (sq != null) Info($"InHand {sq.Model}");
            });

            Clear();
            CreateHandView();
        }

        public void CreateHandView()
        {
            Clear();

            var model = Agent.Model;
            var n = 0;
            foreach (var card in model.Cards)
            {
                var view = Instantiate(CardViewPrefab);
                view.transform.SetParent(CardsRoot);
                view.transform.localPosition = n * Offset;
                view.SetAgent(Agent.Registry.New<ICardAgent>(card));
                view.name = $"{card}";

                ++n;
            }
        }

        [ContextMenu("HandView-Clear")]
        public void Clear()
        {
            foreach (Transform tr in CardsRoot.transform)
                Unity.Destroy(tr);
        }

        private EColor _color;
        private readonly ReactiveProperty<ICardAgent> _hovered = new ReactiveProperty<ICardAgent>();
        private readonly ReactiveProperty<ICardAgent> _hover = new ReactiveProperty<ICardAgent>();
    }
}
