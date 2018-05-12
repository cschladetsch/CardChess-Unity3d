using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Model;
using Flow;
using Flow.Impl;
using UnityEngine.Assertions;

namespace App.Agent
{
    public class Player : AgentCoroBase<Model.IPlayer>, IPlayer
    {
        public EColor Color => _model.Color;

        IFuture<EResponse> IPlayer.AddMaxMana(int mana)
        {
            throw new NotImplementedException();
        }

        public IFuture<EResponse> AddMana(int mana)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerator Next(IGenerator self)
        {
            if (_placeKing != null)
			{
			}

            yield return null;
        }

		protected override bool Create()
        {
            return true;
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
            _model.NewGame();
            var future = New.Future<EResponse>();
            return future;
        }

        public void AddMaxMana(int mana)
        {
            _model.ChangeMaxMana(mana);
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
