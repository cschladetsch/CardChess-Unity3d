using App.Agent;
using Assets.App.View;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.View.Impl1
{
    public class ManaView
        : ViewBase<IPlayerAgent>
        , IManaView
    {
        public ManaElement ManaElementPrefab;
        public Transform Root;
        public Vector2 Offset;

        public override void SetAgent(IPlayerView view, IPlayerAgent agent)
        {
            Assert.IsNotNull(agent);
            Assert.IsNotNull(ManaElementPrefab);
            Assert.IsNotNull(Root);

            base.SetAgent(view, agent);

            Agent.MaxMana.Subscribe(m => Redraw(Agent.Mana.Value, m)).AddTo(this);
            Agent.Mana.Subscribe(m => Redraw(m, Agent.MaxMana.Value)).AddTo(this);

            Agent.Mana.DistinctUntilChanged().Subscribe(n => Redraw()).AddTo(this);
            Agent.MaxMana.DistinctUntilChanged().Subscribe(n => Redraw()).AddTo(this);
        }

        [ContextMenu("ManaBar-Clear")]
        public void Clear()
        {
            foreach (Transform tr in Root)
#if UNITY_EDITOR
                DestroyImmediate(tr.gameObject);
#else
                Destroy(tr.gameObject);
#endif
        }

        [ContextMenu("ManaBar-MockDraw")]
        public void MockDrwa()
        {
            Redraw(2, 5);
        }

        private void Redraw()
        {
            Redraw(Agent.Mana.Value, Agent.MaxMana.Value);
        }

        private void Redraw(int mana, int maxMana)
        {
            Assert.IsTrue(maxMana >= mana);
            Assert.IsTrue(maxMana <= Parameters.MaxManaCap);

            Clear();

            for (var n = 0; n < maxMana; ++n)
            {
                var active = n <= mana;
                var elem = Instantiate(ManaElementPrefab);
                var offset = n * Offset;
                var pos = new Vector3(offset.x, offset.y, 0);
                elem.transform.SetParent(Root);
                elem.transform.localPosition = pos;
                elem.SetAvailable(active);
                elem.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
        }

        private void MakeManaElement(int n, bool active)
        {
        }
    }
}
