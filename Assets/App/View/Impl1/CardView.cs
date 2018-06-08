using App.Agent;
using UniRx;
using UnityEngine;

namespace App.View.Impl1
{
    public class CardView
        : ViewBase<ICardAgent>
        , ICardView
    {
        public TMPro.TextMeshProUGUI Mana;
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public GameObject ModelPrefab;
        public float Width;

        public override void Create()
        {
        }

        public override void SetAgent(ICardAgent agent)
        {
            #if UNITY_EDITOR
            if (agent == null)
                return;
            #endif

            base.SetAgent(agent);

            agent.Power.DistinctUntilChanged().Subscribe(p => Power.text = $"{p}");
            agent.Health.DistinctUntilChanged().Subscribe(p => Health.text = $"{p}");
            agent.Model.ManaCost.DistinctUntilChanged().Subscribe(p => Mana.text = $"{p}");
        }
    }
}
