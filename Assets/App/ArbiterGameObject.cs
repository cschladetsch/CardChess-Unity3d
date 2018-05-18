using System;
using UnityEngine;

namespace App
{
    using Common;
    using Model;
    using Agent;

    public class ArbiterGameObject : MonoBehaviour
    {
        public void Awake()
        {
            var a = _arbiter = new Arbiter();

            var b0 = a.NewModel<BoardModel, int, int>(8, 8);
            var c0 = a.NewAgent<BoardAgent, IBoardModel>(b0);
            var m0 = a.NewModel<PlayerModel, EColor>(EColor.White);
            var m1 = a.NewModel<PlayerModel, EColor>(EColor.Black);
            var p0 = a.NewAgent<PlayerAgent, IPlayerModel>(m0);
            var p1 = a.NewAgent<PlayerAgent, IPlayerModel>(m1);
            var d0 = a.NewModel<DeckModel, Guid, IPlayerModel>(Guid.Empty, m0);
            var d1 = a.NewModel<DeckModel, Guid, IPlayerModel>(Guid.Empty, m1);

            m0.SetDeck(d0);
            m1.SetDeck(d1);

            a.Setup(c0, p0, p1);
        }

        private Arbiter _arbiter;
    }
}
