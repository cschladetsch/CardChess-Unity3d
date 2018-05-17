using System;
using System.Collections;
using System.Linq;
using Flow;

#if VS
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using UnityEngine.Assertions;
#endif

namespace App.Agent
{
    using Action;
    using Common;

    /// <inheritdoc cref="IPlayer" />
    /// <summary>
    /// The agent that represents a player in the game.
    /// </summary>
    public class Player :
        AgentBaseCoro<Model.IPlayer>,
        IPlayer,
        Common.IOwner
    {
        #region Public Fields
        public EColor Color => Model.Color;
        public ICardInstance King { get; private set; }
        public int Health => King.Health;
        public PlayerDeckCollection Deck { get; } = new PlayerDeckCollection();
        public PlayerHandCollection Hand { get; } = new PlayerHandCollection();
        #endregion

        #region Startup Methods
        public IFuture<EResponse> NewGame()
        {
            Info($"{Color}: NewGame");
            Model.NewGame();
            return New.Future(EResponse.Ok);
        }

        public IFuture<int> RollDice()
        {
            return New.Future<int>(_random.Next(0, 6));
        }

        public ITransient StartGame()
        {
            Info($"{Color}: Start Game");
            King = Arbiter.NewAgent<CardInstance, Model.ICardInstance>(Model.King);

            return null;
        }

        public IGenerator DrawInitialCards()
        {
            Info($"{Color}: Draw Initial Cards");
            var cards = Deck.Cards.Take(Parameters.StartHandCardCount);
            foreach (var card in cards)
            {
                Hand.Add(card);
                Deck.Remove(card);
            }

            return null;
        }

        public ITransient Mulligan()
        {
            Info($"{Color}: Mulligan: do nothing");
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

        /// <summary>
        /// For the moment, will create then immediately set a future
        /// for the top card on the deck.
        /// </summary>
        /// <returns></returns>
        public IFuture<ICardInstance> FutureDrawCard()
        {
            if (Model.Deck.Cards.Count == 0)
            {
                Warn($"{Color}: No cards left draw");
                return null;
            }

            _drawCard?.Complete();
            _drawCard = New.NamedFuture<ICardInstance>("DrawCard");
            var card = Deck.DrawTopCard();
            Hand.Add(card);
            _drawCard.Value = card;
            return _drawCard;
        }

        public virtual IFuture<PlayCard> FuturePlaceKing()
        {
            Info($"{Color}: FuturePlaceKing");
            _placeKing?.Complete();
            return _placeKing = New.NamedFuture<PlayCard>("PlaceKing");
        }

        public virtual IFuture<PlayCard> FuturePlayCard()
        {
            Info($"{Color}: FuturePlayCard");
            _playCard?.Complete();
            return _playCard = New.NamedFuture<PlayCard>("PlayCard");
        }

        public virtual IFuture<MovePiece> FutureMovePiece()
        {
            _movePiece?.Complete();
            return _movePiece = New.NamedFuture<MovePiece>("MovePiece");
        }

        public IFuture<bool> FutureAcceptCards()
        {
            Info($"{Color}: FutureAcceptCard");
            _acceptCards?.Complete();
            return _acceptCards = New.NamedFuture<bool>("AcceptCards");
        }

        public IFuture<bool> FuturePass()
        {
            _pass?.Complete();
            return _pass = New.NamedFuture<bool>("Pass");
        }

        public void RedrawCards(params Guid[] rejected)
        {
            throw new NotImplementedException();
        }

        public void AcceptCards()
        {
            Assert.IsNotNull(_acceptCards);
            _acceptCards.Value = true;
        }

        public void PlaceKing(Coord coord)
        {
            Assert.IsNotNull(_placeKing);
            var gen = Arbiter.Board.PlaceCard(King, coord);

            _placeKing.Value = new PlayCard(King, coord);
        }

        public void PlayCard(PlayCard playCard)
        {
            _playCard.Value = playCard;
        }

        public void MovePiece(MovePiece move)
        {
            _movePiece.Value = move;
        }

        public void Pass()
        {
            _pass.Value = true;
        }

        public void CardPlayed(PlayCard playCard)
        {
            Assert.IsNotNull(_playCard);
            _playCard.Value = playCard;
        }

        public void PieceMoved(MovePiece move)
        {
            Assert.IsNotNull(_movePiece);
            _movePiece.Value = move;
        }

        #endregion

        #region Private/Protected Methods

        private ICardInstance Add(Model.ICardInstance model)
        {
            var card = Arbiter.NewCardAgent(model, this);
            Hand.Add(card);
            return null;
        }

        private void Remove(ICardInstance model)
        {
            var inDeck = Deck.Cards.FirstOrDefault(c => c.Model == model);
            Assert.IsNotNull(inDeck);
            Deck.Remove(inDeck);
            Model.Deck.Cards.Remove(Model.Deck.Cards.First(c => c.Template.Id == inDeck.Model.Template.Id));
        }

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
        private IFuture<bool> _acceptCards;
        private IFuture<bool> _pass;
        private IFuture<ICardInstance> _drawCard;
        private IFuture<PlayCard> _placeKing;
        private IFuture<PlayCard> _playCard;
        private IFuture<MovePiece> _movePiece;
        #endregion
    }
}
