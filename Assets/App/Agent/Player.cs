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

        public IFuture<PlayCard> PlaceKing()
        {
            return _placeKing = New.Future<PlayCard>();
        }

        public IFuture<PlayCard> PlayCard()
        {
            var future = New.Future<PlayCard>();
            _cardPlays.Add(future);
            return future;
        }

        public IFuture<MovePiece> MovePiece()
        {
            var future = New.Future<MovePiece>();
            _pieceMoves.Add(future);
            return future;
        }

        public IFuture<bool> Pass()
        {
            throw new NotImplementedException();
        }

        public IFuture<EResponse> NewGame()
        {
            Model.NewGame();
            var future = New.Future<EResponse>();
            return future;
        }

        public IFuture<int> RollDice()
        {
            Assert.IsNull(_roll);
            return _roll = New.Future<int>();
        }

        private IFuture<PlayCard> _placeKing;
        private IFuture<int> _roll;
        private List<IFuture<PlayCard>> _cardPlays = new List<IFuture<PlayCard>>();
        private List<IFuture<MovePiece>> _pieceMoves = new List<IFuture<MovePiece>>();
    }
}
