using UnityEngine;

using UniRx;
using CoLib;

using Dekuple;
using Dekuple.View;
using Dekuple.View.Impl;

namespace App.View.Impl1
{
    using Agent;

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
        public AudioClip MouseOverClip;

        private float _lastClipPlayed;
        private float _minTimeBetweenClips = 0.3f;
        private readonly ReactiveCollection<ICardView> _cards = new ReactiveCollection<ICardView>();

        public override bool IsValid
        {
            get
            {
                Assert.IsNotNull(CardsRoot);
                Assert.IsNotNull(CardViewPrefab);
                Assert.IsNotNull(BoardOverlay);
                foreach (var c in _cards)
                    Assert.IsTrue(c.IsValid);
                return true;
            }
        }

        public override void SetAgent(IViewBase playerView, IHandAgent handAgent)
        {
            base.SetAgent(playerView, handAgent);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            Clear();
            BindHand(handAgent);
            Redraw();
            Assert.IsTrue(IsValid);
        }

        private void BindHand(IHandAgent agent)
        {
            foreach (var card in agent.Cards)
                _cards.Add(CreateViewFromAgent(card));

            agent.Cards.ObserveAdd().Subscribe(Add);
            agent.Cards.ObserveRemove().Subscribe(Remove);
            Assert.IsTrue(IsValid);
        }

        [ContextMenu("Debug")]
        public void Debug()
        {
            foreach (var c in _cards)
            {
                Assert.IsTrue(c.IsValid);
            }
        }

        private ICardView CreateViewFromAgent(ICardAgent agent)
        {
            var cardView = ViewRegistry.FromPrefab<ICardView>(CardViewPrefab);
            cardView.MouseOver.Subscribe(CardMouseOver);
            cardView.SetAgent(OwnerView, agent);
            var tr = cardView.GameObject.transform;
            tr.SetParent(CardsRoot);
            tr.localScale = Vector3.one;
            tr.localPosition = new Vector3(-1, -1, 10);
            Assert.IsTrue(IsValid);
            return cardView;
        }

        private void CardMouseOver(ICardView card)
        {
            if (card == null)
                return;
            if (Time.time - _lastClipPlayed > _minTimeBetweenClips)
            {
                _AudioSource.PlayOneShot(MouseOverClip);
                _lastClipPlayed = Time.time;
            }
            Verbose(20, $"MouseOver {card.Agent.Model}");
        }

        [ContextMenu("HandView-Clear")]
        public void Clear()
        {
            transform.ForEach<ICardView>(c => c.Destroy());
            Assert.IsTrue(IsValid);
        }

        private void Add(CollectionAddEvent<ICardAgent> add)
        {
            Verbose(5, $"HandView: Add {add.Value} @{add.Index}");
            _cards.Insert(add.Index, CreateViewFromAgent(add.Value));
            Redraw();
            Assert.IsTrue(IsValid);
        }

        private void Remove(CollectionRemoveEvent<ICardAgent> remove)
        {
            Verbose(5, $"HandView: Remove {remove.Value} @{remove.Index}");
            var view = _cards[remove.Index];
            _cards.RemoveAt(remove.Index);
            view.Destroy();
            Redraw();
            Assert.IsTrue(IsValid);
        }

        private void Redraw()
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
        }
    }
}
