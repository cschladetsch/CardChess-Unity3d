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

        public override void SetAgent(IPlayerView view, IHandAgent hand)
        {
            base.SetAgent(view, hand);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            Clear();
            CreateHandView();

            // TODO: hand.Cards, not hand.Model.Cards
            hand.Model.Cards.ObserveAdd().Subscribe(Add);
            hand.Model.Cards.ObserveRemove().Subscribe(Remove);

            //_cards.ObserveCountChanged().Subscribe(_ => Redraw());
        }

        public void CreateHandView()
        {
            Clear();

            var model = Agent.Model;
            var n = 0;
            foreach (var card in model.Cards)
            {
                var view = ViewRegistry.FromPrefab<ICardView, ICardAgent, ICardModel>(
                    Player, CardViewPrefab, card);

                Assert.IsTrue(view.IsValid);

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

        private void Redraw()
        {
            //CreateHandView();
        }

        private void Add(CollectionAddEvent<ICardModel> add)
        {
            _cards.Insert(add.Index, Registry.New<ICardView>(add.Value));
        }

        private void Remove(CollectionRemoveEvent<ICardModel> card)
        {
            var view = _cards[card.Index];
            view.Destroy();
            _cards.RemoveAt(card.Index);
        }

        private readonly ReactiveCollection<ICardView> _cards = new ReactiveCollection<ICardView>();
    }
}
