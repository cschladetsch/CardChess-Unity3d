using App.Registry;
using App.Service;
using UnityEngine;

using UniRx;
using CoLib;

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

        public AudioClip MouseOverClip;
        // ReSharper disable once InconsistentNaming
        //[Inject] public IPiecePrefabService _pieceFactory;

        public override bool IsValid
        {
            get
            {
                //Assert.IsNotNull(_pieceFactory);
                Assert.IsNotNull(CardsRoot);
                Assert.IsNotNull(CardViewPrefab);
                Assert.IsNotNull(BoardOverlay);
                foreach (var c in _cards)
                    Assert.IsTrue(c.IsValid);
                return true;
            }
        }

        public override void SetAgent(IPlayerView playerView, IHandAgent handAgent)
        {
            base.SetAgent(playerView, handAgent);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            //Verbosity = 10;
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
            //var prefab = _pieceFactory.GetCardPrefab(agent.Model.PieceType);
            var cardView = ViewRegistry.FromPrefab<ICardView>(CardViewPrefab);
            cardView.MouseOver.Subscribe(CardMouseOver);
            cardView.SetAgent(PlayerView, agent);
            var tr = cardView.GameObject.transform;
            tr.SetParent(CardsRoot);
            tr.localScale = Vector3.one;
            tr.localPosition = new Vector3(-1, -1, 10);
            Assert.IsTrue(IsValid);
            return cardView;
        }

        float _lastClipPlayed;
        private float _minTimeBetweenClips = 0.3f;

        void CardMouseOver(ICardView card)
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
