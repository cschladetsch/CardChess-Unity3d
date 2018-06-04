using App.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

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

        public override bool Construct(IHandAgent hand)
        {
            if (!base.Construct(hand))
                return false;
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            _bitMask = LayerMask.GetMask("CardInHand");
            _hovered.DistinctUntilChanged().Subscribe(sq => _hover.Value = sq);
            Hover.Subscribe(sq => Info($"Hand {sq}"));

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Where(_ => Hover.Value != null)
                .Subscribe(_ => Pickup())
                .AddTo(this)
                ;

            return true;
        }

        protected override void Begin()
        {
            Clear();

            var root = FindObjectOfType<GameRoot>();
            var playerAgent = root.WhiteAgent;
            var playerModel = playerAgent.Model;
            var playerModelHand = playerModel.Hand;
            //var playerAgentHand = playerAgent.Hand;
            var width = GetCardWidth();
            var xs = -playerModelHand.NumCards.Value / 2.0f * width + width/2.0f;
            var n = 0;
            foreach (var card in playerModelHand.Cards)
            {
                var pos = new Vector3(xs + n*width, 0, 0);
                var obj = Instantiate(CardViewPrefab);
                obj.transform.SetParent(CardsRoot);
                obj.transform.localPosition = pos;

                var agentCard = playerAgent.Registry.New<ICardAgent>(card);
                agentCard.Construct(card);
                obj.Construct(agentCard);
            }
        }

        void Pickup()
        {
            Info($"Pickup {Hover.Value}");
        }

        protected override void Step()
        {
            base.Step();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, _bitMask))
            {
                var parent = hit.transform.parent.GetComponent<CardView>();
                _hovered.Value = parent.Agent;
            }
            else
            {
                _hovered.Value = null;
            }
        }

        [ContextMenu("CardHand-MockShow")]
        public void MockShow()
        {
            Clear();

            var width = GetCardWidth();
            var xs = -MockNumCards / 2.0f * width + width/2.0f;

            for (int n = 0; n < MockNumCards; ++n)
            {
                var pos = new Vector3(xs + n*width, 0, 0);
                var card = Instantiate(CardViewPrefab);
                card.transform.SetParent(CardsRoot);
                card.transform.localPosition = pos;
                ICardAgent agent = null;
                card.Construct(agent);
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
        private readonly ReactiveProperty<ICardAgent> _hovered= new ReactiveProperty<ICardAgent>();
        private readonly ReactiveProperty<ICardAgent> _hover = new ReactiveProperty<ICardAgent>();
    }
}
