using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Agent;
using UnityEngine;

namespace App.View.Impl1
{
    public class PlayerView
        : ViewBase<IPlayerAgent>
        , IPlayerView
    {
        public ManaBar ManaBar;
        public IHandView Hand;
        public IDeckView Deck;

        public override bool Construct(IPlayerAgent agent)
        {
            if (!base.Construct(agent))
                return false;

            ManaBar = FindChild<ManaBar>();
            Hand = FindChild<IHandView>();
            Deck = FindChild<IDeckView>();

            if (ManaBar == null)
                return false;
            if (Hand == null)
                return false;
            if (Deck == null)
                return false;

            Deck.Construct(Agent.Deck);
            Hand.Construct(Agent.Hand);
            //ManaBar.Construct(Agent);

            return false;
        }

        T FindChild<T>() where T : class
        {
            foreach (Transform ch in transform)
            {
                foreach (var comp in ch.GetComponents<Component>())
                {
                    if (comp is T)
                    {
                        return comp as T;
                    }
                }
            }

            return null;
        }
    }
}
