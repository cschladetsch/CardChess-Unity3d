using UnityEngine;
using UnityEngine.Assertions;

namespace App.View.Impl
{
    using Agent;

    public class HandView
        : ViewBase<IHandAgent>
    {
        public CardView CardViewPrefab;
        public Transform CardsRoot;
        public int MockNumCards = 4;

        protected override bool Create()
        {
            Assert.IsNotNull(CardViewPrefab);
            Assert.IsNotNull(CardsRoot);

            return base.Create();
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
    }
}
