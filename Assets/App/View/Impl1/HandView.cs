using UnityEngine;

using UniRx;

namespace App.View.Impl1
{
    using Agent;
    using Common;
    using Model;

    public class HandView
        : ViewBase<IHandAgent>
        , IHandView
    {
        public CardView CardViewPrefab;
        public Transform CardsRoot;
        public int MockNumCards = 4;
        public Vector3 Offset;

        public override void SetAgent(IPlayerView view, IHandAgent handAgent)
        {
            base.SetAgent(view, handAgent);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            Clear();
            CreateHandView();

            //handAgent.Cards.ObserveAdd().Subscribe(Add);
            //handAgent.Cards.ObserveRemove().Subscribe(Remove);
        }

        public void CreateHandView()
        {
            Clear();

            var model = Agent.Model;
            var n = 0;
            foreach (var card in model.Cards)
            {
                var view = ViewRegistry.FromPrefab<ICardView, ICardAgent, ICardModel>(PlayerView, CardViewPrefab, card);
                var tr = view.GameObject.transform;
                tr.SetParent(CardsRoot);
                tr.localPosition = n * Offset;
                view.GameObject.name = $"{card}";
                _cards.Add(view);
                ++n;
            }
        }

        [ContextMenu("HandView-Clear")]
        public void Clear()
        {
            foreach (Transform tr in CardsRoot.transform)
                Unity.Destroy(tr);
        }

        private void Add(CollectionAddEvent<ICardAgent> add)
        {
            Info($"HandView: Add {add.Value} @{add.Index}");
            var cardView = ViewRegistry.New<ICardView>();
            cardView.SetAgent(PlayerView, add.Value);
            _cards.Insert(add.Index, cardView);
        }

        private void Remove(CollectionRemoveEvent<ICardAgent> remove)
        {
            Info($"HandView: Remove {remove.Value} @{remove.Index}");
            var view = _cards[remove.Index];
            view.Destroy();
            _cards.RemoveAt(remove.Index);
        }

        private readonly ReactiveCollection<ICardView> _cards = new ReactiveCollection<ICardView>();
    }
}
