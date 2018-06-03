using App.Agent;
using UniRx;
using UnityEngine.UI;

namespace App.View.Impl
{

    public class CardView
        : ViewBase<ICardAgent>
    {
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;

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
