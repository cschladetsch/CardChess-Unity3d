using App.Agent;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

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

        protected override bool Create()
        {
            if (!base.Create())
                return false;
            return true;
        }

        public override bool Construct(ICardAgent agent)
        {
            if (!base.Construct(agent))
                return false;

            Agent.Health.DistinctUntilChanged(h => Health.text = $"Health: {h}");
            return true;
        }
    }
}
