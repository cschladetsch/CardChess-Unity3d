using UnityEngine;

namespace App.View.Impl1
{
    using Agent;

    public class DeckView
        : ViewBase<IDeckAgent>
        , IDeckView
    {
        public Transform CardsRoot;
        public CardView CardViewPrefab;

        protected override void Begin()
        {
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
