using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Agent
{
    using Action;

    /// <inheritdoc cref="IPlayer" />
    /// <summary>
    /// The agent that represents a player in the game.
    /// </summary>
    public class Player : AgentBaseCoro<Model.IPlayer>, IPlayer
    {
        #region Public Fields
        public EColor Color => Model.Color;
        public ICardInstance King { get; private set; }
        public int Health => King.Health;
        public PlayerDeckCollection Deck { get; } = new PlayerDeckCollection();
        public PlayerHandCollection Hand { get; } = new PlayerHandCollection();
        #endregion

        #region Public Methods
        public virtual IFuture<PlayCard> PlaceKing()
        {
            _placeKing?.Complete();
            return _placeKing = New.NamedFuture<PlayCard>("PlaceKing");
        }

        public virtual IFuture<PlayCard> PlayCard()
        {
            _playCard?.Complete();
            return _playCard = New.NamedFuture<PlayCard>("PlayCard");
        }

        public virtual IFuture<MovePiece> MovePiece()
        {
            _movePiece?.Complete();
            return _movePiece = New.NamedFuture<MovePiece>("MovePiece");
        }

        public void RedrawCards(params Guid[] rejected)
        {
            //var hand = Hand;
            //var removed = new List<Agent.ICardInstance>();
            //foreach (var rej in rejected)
            //{
            //    removed.Add(hand.Cards.First(c => c.Template.Id == rej));
            //}
        }

        public void PlaceKing(Coord coord)
        {
        }

        public void PlayCard(PlayCard playCard)
        {
            _playCard.Value = playCard;
        }

        public void MovePiece(MovePiece move)
        {
            _movePiece.Value = move;
        }

        public ITransient AcceptCards()
        {
            Assert.IsNotNull(_hasAccepted);
            _hasAccepted.Complete();
            return _hasAccepted;
        }

        #endregion

        #region Public Flow Methods
        public IFuture<EResponse> NewGame()
        {
            Model.NewGame();
            King = Arbiter.NewAgent<CardInstance, Model.ICardInstance>(Model.King);
            return New.Future(EResponse.Ok);
        }

        public ITransient StartGame()
        {
            //TODO Info("Start Game");
            return null;
        }

        public IGenerator DrawInitialCards()
        {
            //TODO Info("DrawCards");
            var deck = Model.Deck.Cards;
            var hand = Model.Hand.Cards;
            foreach (var card in deck.Take(App.Model.Player.StartHandCardCount))
            {
                hand.Add(card);
                deck.Remove(card);
            }

            return _hasAccepted = New.Node("DrawCards");
        }

        public ITransient Mulligan()
        {
            _hasAccepted.Complete();
            return null;
        }

        public IFuture<EResponse> ChangeMaxMana(int delta)
        {
            Model.ChangeMaxMana(delta);
            return New.Future(EResponse.Ok);
        }

        public IFuture<EResponse> ChangeMana(int delta)
        {
            Model.ChangeMana(delta);
            return New.Future(EResponse.Ok);
        }

        public ITransient StartDrawCard()
        {
            return null;
        }

        public IFuture<PlayCard> StartPlayCard()
        {
            _playCard?.Complete();
            return _playCard = New.NamedFuture<PlayCard>("PlayCard");
        }

        public IFuture<MovePiece> StartMovePiece()
        {
            _movePiece?.Complete();
            return _movePiece = New.NamedFuture<MovePiece>("MovePiece");
        }

        public IFuture<bool> Pass()
        {
            return New.Future(false);
        }

        public void KingPlaced(Coord coord)
        {
            _placeKing = New.NamedFuture<PlayCard>(
                "HasPlacedKing", new Action.PlayCard(King, coord));
        }

        public void CardPlayed(PlayCard playCard)
        {
            Assert.IsNotNull(_playCard);
            _playCard.Value = playCard;
        }

        public void PieceMoved(MovePiece move)
        {
            throw new NotImplementedException();
        }

        public void Passed()
        {
            throw new NotImplementedException();
        }

        public ITransient DrawCard()
        {
            if (Model.Deck.Cards.Count == 0)
            {
                //Warn("No cards left");
                return null;
            }

            var card = Model.Deck.Cards.First();
            Model.Hand.Add(card);
            Model.Deck.Cards.Remove(card);
            return null;
        }

        public IFuture<int> RollDice()
        {
            return New.Future<int>(_random.Next(0, 6));
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// The over-time actions of this entity.
        /// </summary>
        /// <param name="self">The context</param>
        /// <returns></returns>
        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }
        #endregion

        #region Private Fields
        private readonly Random _random = new Random();

        private IFuture<int> _roll;
        private IGenerator _hasAccepted;
        private IFuture<PlayCard> _placeKing;
        private IFuture<PlayCard> _playCard;
        private IFuture<MovePiece> _movePiece;
        #endregion
    }
}
