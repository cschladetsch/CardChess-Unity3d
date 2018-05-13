using System;
using System.Collections;
using System.Collections.Generic;
using Flow;
using UnityEngine.Assertions;

namespace App.Agent
{
    using Action;

    public class Player : AgentBaseCoro<Model.IPlayer>, IPlayer
    {
        public EColor Color => Model.Color;
        public ICardInstance King { get; private set; }

        public IFuture<EResponse> NewGame()
        {
            Model.NewGame();
            King = Main.Arbiter.Instance.NewAgent<CardInstance, Model.ICardInstance>(Model.King);
            var future = New.Future<EResponse>();
            future.Value = EResponse.Ok;
            return future;
        }

        public IFuture<Response> Mulligan()
        {
            return null;
        }

        public IFuture<EResponse> ChangeMaxMana(int mana)
        {
            var future = New.Future<EResponse>();
            Model.ChangeMaxMana(mana, (response) => future.Value = response);
            return future;
        }

        public IFuture<EResponse> ChangeMana(int mana)
        {
            var future = New.Future<EResponse>();
            Model.ChangeMana(mana, (response) => future.Value = response);
            return future;
        }

        protected override IEnumerator Next(IGenerator self)
        {
            if (_placeKing != null)
            {
            }

            yield return null;
        }

        public virtual IFuture<PlayCard> PlaceKing()
        {
            return _placeKing = New.Future<PlayCard>();
        }

        public virtual IFuture<PlayCard> PlayCard()
        {
            var future = New.Future<PlayCard>();
            _cardPlays.Add(future);
            return future;
        }

        public virtual IFuture<MovePiece> MovePiece()
        {
            var future = New.Future<MovePiece>();
            _pieceMoves.Add(future);
            return future;
        }

        public IFuture<bool> Pass()
        {
            var pass = New.Future<bool>();
            pass.Value = false;
            return pass;
        }

        public IFuture<int> RollDice()
        {
            var roll = New.Future<int>();
            roll.Value = _random.Next(0, 6);
            return roll;
        }

        private System.Random _random = new Random();
        private IFuture<PlayCard> _placeKing;
        private IFuture<int> _roll;
        private List<IFuture<PlayCard>> _cardPlays = new List<IFuture<PlayCard>>();
        private List<IFuture<MovePiece>> _pieceMoves = new List<IFuture<MovePiece>>();
    }
}
