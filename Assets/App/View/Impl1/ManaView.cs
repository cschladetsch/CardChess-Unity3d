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
        public IPlayerAgent Player;
        public ManaElement ManaElementPrefab;
        public Transform Root;
        public bool Reverse;

        public override void SetAgent(IPlayerAgent agent)
        {
            Assert.IsNotNull(agent);
            Assert.IsNotNull(ManaElementPrefab);
            Assert.IsNotNull(Root);

            base.SetAgent(agent);

            Player.MaxMana.Subscribe(m => Redraw(Player.Mana.Value, m)).AddTo(this);
            Player.Mana.Subscribe(m => Redraw(m, Player.MaxMana.Value)).AddTo(this);

            Player.Mana.DistinctUntilChanged().Subscribe(n => Redraw()).AddTo(this);
            Player.MaxMana.DistinctUntilChanged().Subscribe(n => Redraw()).AddTo(this);
        }

        public void Clear()
        {
            foreach (Transform tr in Root)
                Destroy(tr.gameObject);
        }

        [ContextMenu("ManaBar-MockDraw")]
        public void MockDrwa()
        {
            Redraw(2, 5);
        }

        private void Redraw()
        {
            Redraw(Player.Mana.Value, Player.MaxMana.Value);
        }

        private void Redraw(int mana, int maxMana)
        {
            Assert.IsTrue(maxMana >= mana);
            Assert.IsTrue(maxMana <= Parameters.MaxManaCap);

            Clear();
            for (var n = 0; n < maxMana; ++n)
                MakeManaElement(n, n <= mana);
        }

        private void MakeManaElement(int n, bool active)
        {
            var elem = Instantiate(ManaElementPrefab);
            var sign = Reverse ? -1 : 1;
            var pos = new Vector3(n * elem.Width*sign, 0, 0);
            elem.transform.SetParent(Root);
            elem.transform.localPosition = pos;
            elem.SetActive(active);
            elem.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }
}
