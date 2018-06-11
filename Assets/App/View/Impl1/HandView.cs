using System.Collections.Generic;
using System.Linq;
using CoLib;
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

        public override void SetAgent(IPlayerView playerView, IHandAgent handAgent)
        {
            base.SetAgent(playerView, handAgent);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            Clear();

            foreach (var cardAgent in handAgent.Cards)
                _cards.Add(ViewFromAgent(cardAgent));
            handAgent.Cards.ObserveAdd().Subscribe(Add);
            handAgent.Cards.ObserveRemove().Subscribe(Remove);
            Redraw();
        }

        ICardView ViewFromAgent(ICardAgent agent)
        {
            var cardView = ViewRegistry.FromPrefab<ICardView>(CardViewPrefab);
            cardView.SetAgent(PlayerView, agent);
            var tr = cardView.GameObject.transform;
            tr.SetParent(CardsRoot);
            tr.localScale = Vector3.one;
            tr.localPosition = new Vector3(-1, -1, 10);
            return cardView;
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
            _cards.Insert(add.Index, ViewFromAgent(add.Value));
            Redraw();
        }

        private void Remove(CollectionRemoveEvent<ICardAgent> remove)
        {
            Info($"HandView: Remove {remove.Value} @{remove.Index}");
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
