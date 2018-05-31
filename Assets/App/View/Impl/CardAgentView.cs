using App.Agent;
using UniRx;
using UnityEngine.UI;

namespace App.View.Impl
{

    public class CardAgentView
        : ViewBase<ICardAgent>
    {
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;

        public Button Give;
        public Button Take;

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

            Give.Bind(() => Agent.ChangeHealth(10));
            Take.Bind(() => Agent.ChangeHealth(-10));

            Agent.Health.DistinctUntilChanged(h => Health.text = $"Health: {h}");
            return true;
        }
    }
}
