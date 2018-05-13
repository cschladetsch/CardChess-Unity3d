using System;
using UnityEngine;

namespace App.Main
{
    public class ArbiterGameObject : MonoBehaviour
    {
        public void Awake()
        {
            var a = _arbiter = new Arbiter();

            var b0 = a.NewModel<Model.Board, int, int>(8, 8);
            var c0 = a.NewAgent<Agent.Board, Model.IBoard>(b0);
            var m0 = a.NewModel<Model.Player, EColor>(EColor.White);
            var m1 = a.NewModel<Model.Player, EColor>(EColor.Black);
            var p0 = a.NewAgent<Agent.Player, Model.IPlayer>(m0);
            var p1 = a.NewAgent<Agent.Player, Model.IPlayer>(m1);
            var d0 = a.NewModel<Model.Deck, Guid, Model.IPlayer>(Guid.Empty, m0);
            var d1 = a.NewModel<Model.Deck, Guid, Model.IPlayer>(Guid.Empty, m1);

            m0.SetDeck(d0);
            m1.SetDeck(d1);

            a.Setup(c0, p0, p1);
        }

        private Arbiter _arbiter;
    }
}
