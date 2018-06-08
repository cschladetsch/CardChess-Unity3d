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
                //CurrentHover.DistinctUntilChanged().Subscribe(Hover);

                ++n;
            }
        }

        private void Hover(ICardView card)
        {
            _Queue.Enqueue(
                Commands.ScaleTo(
                    card.GameObject,
                    1.5f,
                    1.0
                )
            );
        }

        [ContextMenu("HandView-Clear")]
        public void Clear()
        {
            foreach (Transform tr in CardsRoot.transform)
                Unity.Destroy(tr);
        }
    }
}
