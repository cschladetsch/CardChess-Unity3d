using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flow;

namespace App.Agent
{
    using Action;

    /// <inheritdoc />
    /// <summary>
    /// The agent that represents a player in the game.
    /// </summary>
    public class Player : AgentBaseCoro<Model.IPlayer>, IPlayer
    {
        public EColor Color => Model.Color;
        public ICardInstance King { get; private set; }
        public int Health => King.Health;

        public IFuture<EResponse> NewGame()
        {
            Model.NewGame();
            King = Main.Arbiter.Instance.NewAgent<CardInstance, Model.ICardInstance>(Model.King);
            var future = New.Future<EResponse>();
            future.Value = EResponse.Ok;
            return future;
        }

        public ITransient StartGame()
        {
            return null;
        }

        public IFuture<EResponse> DrawInitialCards()
        {
            foreach (var card in Model.Deck.Cards.Take(App.Model.Player.StartHandCardCount))
            {
                Model.Hand.Cards.Add(card);
                Model.Deck.Cards.Remove(card);
            }

            return New.Future(EResponse.Ok);
        }

        public ITransient Mulligan()
        {
            return null;
        }

        public IFuture<EResponse> ChangeMaxMana(int mana)
        {
            Model.ChangeMaxMana(mana);
            return New.Future(EResponse.Ok);
        }

        public IFuture<EResponse> ChangeMana(int mana)
        {
            Model.ChangeMana(mana);
            return New.Future(EResponse.Ok);
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

        private readonly Random _random = new Random();
        private IFuture<PlayCard> _placeKing;
        private IFuture<int> _roll;
        private readonly List<IFuture<PlayCard>> _cardPlays = new List<IFuture<PlayCard>>();
        private readonly List<IFuture<MovePiece>> _pieceMoves = new List<IFuture<MovePiece>>();
    }
}
