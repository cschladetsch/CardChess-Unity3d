using App.Agent;
using UnityEngine;

namespace App.View.Impl1
{
    public class CardView
        : ViewBase<ICardAgent>
        , ICardView
    {
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public GameObject ModelPrefab;
        public float Width;

        public override void Create()
        {
        }

        public override void SetAgent(ICardAgent model)
        {
            base.SetAgent(model);
        }
    }
}
