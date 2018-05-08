using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Adic;
using CoLib;
using Flow;

namespace App.View.World
{
    /// <summary>
    /// Responsible for all visual and audio animations that occur as a result of 
    /// decisions made by the arbiter.
    /// </summary>
    public class ArbiterView : Agent
    {
        public Model.Board Board { get { return _arbiter.Board; } }

        public ArbiterView(Arbiter arbiter)
        {
            _arbiter = arbiter;
        }

        public ITransient PlayerLost(IPlayer loser)
        {
            throw new System.NotImplementedException();
        }

        public ITransient PlayerTimedOut(IPlayer player)
        {
            throw new System.NotImplementedException();
        }

        public ITransient InvalidPlay(PlayCard playCard)
        {
            throw new System.NotImplementedException();
        }

        public ITransient InvalidMove(MovePiece move)
        {
            throw new System.NotImplementedException();
        }

        public ITransient PlayCard(PlayCard playCard)
        {
            throw new System.NotImplementedException();
        }

        public ITransient MovePiece(MovePiece move)
        {
            throw new System.NotImplementedException();
        }

        public ITransient PlayerInCheck(IPlayer player)
        {
            throw new System.NotImplementedException();
        }

        public ITransient NextPlayerTurn(IPlayer previous, IPlayer currentPlayer)
        {
            throw new System.NotImplementedException();
        }

        private Arbiter _arbiter;
    }
}
