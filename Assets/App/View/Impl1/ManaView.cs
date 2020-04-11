<<<<<<< HEAD
﻿using Dekuple.Agent;
using Dekuple.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

using Dekuple.View;
using Dekuple.View.Impl;

namespace App.View.Impl1
=======
﻿namespace App.View.Impl1
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
{
    using UnityEngine;
    using UnityEngine.Assertions;
    using UniRx;
    using Dekuple.View;
    using Dekuple.View.Impl;
    using Agent;

    /// <summary>
    /// View of the set of mana available to a player
    /// </summary>
    public class ManaView
        : ViewBase<IPlayerAgent>
        , IManaView
    {
        public ManaElement ManaElementPrefab;
        public Transform Root;
        public Vector2 Offset;

        public override void SetAgent(IAgent agent)
        {
            Assert.IsNotNull(agent);
            Assert.IsNotNull(ManaElementPrefab);
            Assert.IsNotNull(Root);

            base.SetAgent(agent);

            Agent.Mana.Subscribe(n => Redraw()).AddTo(this);
            Agent.MaxMana.Subscribe(n => Redraw()).AddTo(this);
        }

        [ContextMenu("ManaBar-Clear")]
        public void Clear()
        {
            foreach (Transform tr in Root)
                Destroy(tr.gameObject);
        }

        [ContextMenu("ManaBar-MockDraw")]
        public void MockDraw()
        {
            Redraw(2, 5);
        }

        [ContextMenu("ManaBar-Draw")]
        public void Redraw()
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
                var active = n < mana;
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
