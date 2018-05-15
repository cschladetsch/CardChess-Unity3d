using System;
using System.Collections;
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
    public class Player :
        AgentBaseCoro<Model.IPlayer>,
        IPlayer,
        IOwner
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
            Model.NewGame();
            King = Arbiter.NewAgent<CardInstance, Model.ICardInstance>(Model.King);
            return New.Future(EResponse.Ok);
        }

        public IFuture<int> RollDice()
        {
            return New.Future<int>(_random.Next(0, 6));
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
            foreach (var model in deck.Take(App.Model.Player.StartHandCardCount))
            {
                hand.Add(model);
                deck.Remove(model);

                Deck.Add(NewCardAgent(model));
            }
            return null;
        }

        public ITransient Mulligan()
        {
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

        public IFuture<ICardInstance> FutureDrawCard()
        {
            if (Model.Deck.Cards.Count == 0)
            {
                //Warn("No cards left");
                return null;
            }

            var model = Model.Deck.Cards.First();

            ModelTransferFromDeckToHand(model);

            return New.Future(AddToHand(model));
        }

        public virtual IFuture<PlayCard> FuturePlaceKing()
        {
            _placeKing?.Complete();
            return _placeKing = New.NamedFuture<PlayCard>("PlaceKing");
        }

        public virtual IFuture<PlayCard> FuturePlayCard()
        {
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
        private void ModelTransferFromDeckToHand(Model.ICardInstance model)
        {
            Assert.IsTrue(Model.Deck.Cards.Contains(model));
            Model.Deck.Cards.Remove(model);
            Model.Hand.Add(model);
        }

        private ICardInstance AddToHand(Model.ICardInstance model)
        {
            var card = NewCardAgent(model);
            Hand.Add(card);
            return card;
        }

        private void RemoveFromAgentDeck(Model.ICardInstance model)
        {
            var inDeck = Deck.Cards.FirstOrDefault(c => c.Model == model);
            Assert.IsNotNull(inDeck);
            Deck.Remove(inDeck);
        }

        private ICardInstance NewCardAgent(Model.ICardInstance model)
        {
            return Arbiter.NewCardAgent(model, this);
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
        private IFuture<PlayCard> _placeKing;
        private IFuture<PlayCard> _playCard;
        private IFuture<MovePiece> _movePiece;
        #endregion
    }
}
