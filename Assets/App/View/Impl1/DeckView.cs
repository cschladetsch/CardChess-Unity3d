using UnityEngine;

namespace App.View.Impl1
{
    using Agent;
    using Common;

    public class DeckView
        : ViewBase<IDeckAgent>
        , IDeckView
    {
        public Transform CardsRoot;
        public CardView CardViewPrefab;
        public float DeltaX = 0.2f;

        protected override void Begin()
        {
            Clear();
        }

        public override void SetAgent(IPlayerView view, IDeckAgent agent)
        {
            base.SetAgent(view, agent);
        }

        void Clear()
        {
            foreach (Transform tr in CardsRoot)
                Destroy(tr.gameObject);
        }

        [ContextMenu("DeckView-FromModel")]
        void ShowDeck()
        {
            Clear();

            var nx = 0;
            var owner = Agent.Owner.Value as IPlayerAgent;
            Assert.IsNotNull(owner);
            var dx = owner.Color == EColor.Black ? DeltaX : -DeltaX;
            foreach (var card in Agent.Model.Cards)
            {
                var view = Instantiate(CardViewPrefab);
                view.SetAgent(Player, Agent.Registry.New<ICardAgent>(card));
                view.transform.SetParent(CardsRoot);
                view.transform.localPosition = new Vector3(nx * dx, 0, 0);
                view.transform.localRotation = Quaternion.Euler(0, 90, 0);
                ++nx;
            }
        }

        [ContextMenu("DeckView-MockShow")]
        public void MockShow()
        {
            var card = Instantiate(CardViewPrefab);
            card.transform.SetParent(CardsRoot);
            card.transform.position = Vector3.zero;
        }
    }
}
