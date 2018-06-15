using CoLib;
using UnityEngine;

using UniRx;

namespace App.View.Impl1
{
    using Agent;
    using Common;

    /// <summary>
    /// View of a player's hand in the scene.
    /// </summary>
    public class HandView
        : ViewBase<IHandAgent>
        , IHandView
    {
        public Vector3 Offset;
        public Transform CardsRoot;
        public CardView CardViewPrefab;
        public BoardOverlayView BoardOverlay;

        public override void SetAgent(IPlayerView playerView, IHandAgent handAgent)
        {
            base.SetAgent(playerView, handAgent);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            Clear();
            BindHand(handAgent);
            Redraw();
        }

        private void BindHand(IHandAgent agent)
        {
            foreach (var card in agent.Cards)
                _cards.Add(CreateViewFromAgent(card));

            agent.Cards.ObserveAdd().Subscribe(Add);
            agent.Cards.ObserveRemove().Subscribe(Remove);
        }

        private ICardView CreateViewFromAgent(ICardAgent agent)
        {
            var cardView = ViewRegistry.FromPrefab<ICardView>(CardViewPrefab);
            cardView.MouseOver.Subscribe(CardMouseOver);
            cardView.SetAgent(PlayerView, agent);
            var tr = cardView.GameObject.transform;
            tr.SetParent(CardsRoot);
            tr.localScale = Vector3.one;
            tr.localPosition = new Vector3(-1, -1, 10);
            return cardView;
        }

        void CardMouseOver(ICardView card)
        {
            if (card == null)
                return;
            Info($"MouseOver {card.Agent.Model}");
        }

        [ContextMenu("HandView-Clear")]
        public void Clear()
        {
            foreach (Transform tr in CardsRoot.transform)
                Unity.Destroy(tr);
        }

        private void Add(CollectionAddEvent<ICardAgent> add)
        {
            //Info($"HandView: Add {add.Value} @{add.Index}");
            _cards.Insert(add.Index, CreateViewFromAgent(add.Value));
            Redraw();
        }

        private void Remove(CollectionRemoveEvent<ICardAgent> remove)
        {
            //Info($"HandView: Remove {remove.Value} @{remove.Index}");
            var view = _cards[remove.Index];
            _cards.RemoveAt(remove.Index);
            view.Destroy();
            Redraw();
        }

        void Redraw()
        {
            _Queue.RunToEnd();

            var n = 0;
            foreach (var card in _cards)
            {
                Assert.IsTrue(card.IsValid);
                card.GameObject.name = $"{card.Agent.Model}";
                _Queue.Enqueue(Commands.MoveTo(card.GameObject, n * Offset, 0.1, Ease.Smooth(), true));
                ++n;
            }
            //_Queue.Enqueue(Commands.ForEachParallel(moves));
        }

        private readonly ReactiveCollection<ICardView> _cards = new ReactiveCollection<ICardView>();
    }
}
