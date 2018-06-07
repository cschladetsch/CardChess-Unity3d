using System;
using App.Common;
using UniRx;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace App.View.Impl1
{
    using Agent;

    public class HandView
        : ViewBase<IHandAgent>
        , IHandView
    {
        public CardView CardViewPrefab;
        public Transform CardsRoot;
        public int MockNumCards = 4;
        public IReactiveProperty<ICardAgent> Hover => _hover;

        private EColor _color;

        public override void SetAgent(IHandAgent hand)
        {
            base.SetAgent(hand);
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            _color = hand.Owner.Value.Color;

            _bitMask = LayerMask.GetMask("CardInHand");
            _hovered
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromSeconds(0.05))
                .Subscribe(sq => _hover.Value = sq);
            Hover.Subscribe(sq =>
            {
                if (sq != null) Info($"InHand {sq.Model}");
            });

            //Observable.EveryUpdate()
            //    .Where(_ => Input.GetMouseButtonDown(0))
            //    .Throttle(TimeSpan.FromSeconds(0.05))
            //    .Where(_ => Hover.Value != null && !_dragging)
            //    .Subscribe(_ => Pickup())
            //    .AddTo(this)
            //    ;

            CreateHandView();
        }

        private bool _dragging;
        private ICardAgent _cardDragged;

        void Pickup()
        {
            Info($"Pickup {Hover.Value.Model}");
            _cardDragged = Hover.Value;
        }

        void ReturnToHand()
        {
            Info($"ReturnToHand {Hover.Value.Model}");
        }

        [ContextMenu("HandView-FromModel")]
        public void CreateHandView()
        {
            Clear();

            var width = GetCardWidth();
            var model = Agent.Model;
            var xs = -model.NumCards.Value / 2.0f * width + width / 2.0f;
            var n = 0;
            foreach (var card in model.Cards)
            {
                var view = Instantiate(CardViewPrefab);
                view.transform.SetParent(CardsRoot);
                view.transform.localPosition = new Vector3(xs + n * width, 0, 0);
                view.SetAgent(Agent.Registry.New<ICardAgent>(card));
                view.name = $"{model}";

                ++n;
            }
        }

        private Vector3 screenPoint;
        private Vector3 offset;

        void OnMouseDown()
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }

        void OnMouseDrag()
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            transform.position = cursorPosition;
        }

        private void Update()
        {
            base.Step();

            TestHoverCard();
        }

        private void TestHoverCard()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, _bitMask))
            {
                var parent = hit.transform.parent.GetComponent<CardView>();
                if (parent.Owner.Value.Color == _color)
                    _hovered.Value = parent.Agent;
            }
            else
            {
                _hovered.Value = null;
            }
        }

        [ContextMenu("HandView-MockShow")]
        public void MockShow()
        {
            Clear();

            var width = GetCardWidth();
            var xs = -MockNumCards / 2.0f * width + width / 2.0f;

            for (int n = 0; n < MockNumCards; ++n)
            {
                var pos = new Vector3(xs + n * width, 0, 0);
                var card = Instantiate(CardViewPrefab);
                card.transform.SetParent(CardsRoot);
                card.transform.localPosition = pos;
                ICardAgent model = null;
                card.SetAgent(model);
            }
        }

        private float GetCardWidth()
        {
            // has to be a better way...
            var first = Instantiate(CardViewPrefab);
            var width = first.Width;
            first.gameObject.SetActive(false);
            Destroy(first.gameObject);
            return width;
        }

        [ContextMenu("CardHand-Clear")]
        public void Clear()
        {
            foreach (Transform tr in CardsRoot.transform)
            {
                Destroy(tr.gameObject);
            }
        }

        private int _bitMask;
        private readonly ReactiveProperty<ICardAgent> _hovered = new ReactiveProperty<ICardAgent>();
        private readonly ReactiveProperty<ICardAgent> _hover = new ReactiveProperty<ICardAgent>();
    }
}
