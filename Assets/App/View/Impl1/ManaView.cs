using System.Collections.Generic;
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

        public override bool Construct(IPlayerAgent agent)
        {
            if (!base.Construct(agent))
                return false;

            Player.MaxMana.Subscribe(m => Redraw(Player.Mana.Value, m)).AddTo(this);
            Player.Mana.Subscribe(m => Redraw(m, Player.MaxMana.Value)).AddTo(this);

            return true;
        }

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
            var n = 0;
            for (; n < maxMana; ++n)
                MakeManaElement(n, n <= mana);
        }

        private ManaElement MakeManaElement(int n, bool active)
        {
            var elem = Instantiate(ManaElementPrefab);
            var sign = Reverse ? -1 : 1;
            var pos = new Vector3(n * elem.Width*sign, 0, 0);
            elem.transform.SetParent(Root);
            elem.transform.localPosition = pos;
            elem.SetActive(active);
            elem.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            return elem;
        }
    }
}
