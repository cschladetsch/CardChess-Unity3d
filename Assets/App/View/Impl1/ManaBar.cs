using System.Collections.Generic;
using App.Agent;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.View.Impl1
{
    public class ManaBar
        : ViewBase
    {
        public IPlayerAgent Player;
        public ManaElement ManaElementPrefab;
        public Transform Root;

        protected override void Begin()
        {
            Assert.IsNotNull(ManaElementPrefab);
            Assert.IsNotNull(Root);
            base.Begin();

            if (Player != null)
            {
                Player.Mana.DistinctUntilChanged().Subscribe(n => Redraw());
                Player.MaxMana.DistinctUntilChanged().Subscribe(n => Redraw());
            }
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

        void Redraw()
        {
            Redraw(Player.Mana.Value, Player.MaxMana.Value);
        }

        void Redraw(int mana, int maxMana)
        {
            Assert.IsTrue(maxMana >= mana);
            Assert.IsTrue(maxMana <= Parameters.MaxManaCap);

            Clear();
            _manaIndicators = new List<ManaElement>(maxMana);
            var n = 0;
            for (; n < maxMana; ++n)
                MakeManaElement(n, n <= mana);
        }

        private ManaElement MakeManaElement(int n, bool active)
        {
            var elem = Instantiate(ManaElementPrefab);
            var pos = new Vector3(n * elem.Width, 0, 0);
            elem.transform.SetParent(Root);
            elem.transform.localPosition = pos;
            elem.SetActive(active);
            elem.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            return elem;
        }

        private List<ManaElement> _manaIndicators;
    }
}
