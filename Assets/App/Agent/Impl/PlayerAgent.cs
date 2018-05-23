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

    /// <inheritdoc cref="IPlayerAgent" />
    /// <summary>
    /// The agent that represents a playerAgent in the game.
    /// </summary>
    public class PlayerAgent :
        AgentBaseCoro<Model.IPlayerModel>,
        IPlayerAgent,
        Common.IOwner
    {
        #region Public Fields
        public EColor Color => Model.Color;
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;
        public ICardAgent King { get; private set; }
        public int Health => King.Health;
        public IDeckAgent Deck { get; private set; }
        public IHandAgent Hand { get; private set; }
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
            // TODO: Arbiter will need its own Registry-type thing combined with
            // Model.Registry and Flow.IFactory
            //King = Arbiter.NewAgent<Agent.Card.King, Model.ICardModel>(Model.King);

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
        /// for the top card on the deckModel.
        /// </summary>
        /// <returns></returns>
        public IFuture<ICardAgent> FutureDrawCard()
        {
            _drawCard?.Complete();
            _drawCard = Deck.Draw();
            Node.Add(New.Coroutine(FutureDrawCardCoro));
            return _drawCard;
        }

        IEnumerator FutureDrawCardCoro(IGenerator self)
        {
            if (Model.Deck.NumCards == 0)
            {
                Warn($"{Color}: No cards left draw");
                yield break;
            }

            yield return self.After(_drawCard);
            if (!_drawCard.Available)
            {
                Error("Failed to draw card");
                yield break;
            }
            var agent = _drawCard.Value as ICardAgent;
            if (agent == null)
            {
                Error($"Expected a ICardAgent, got {agent.GetType().Name}");
                yield break;
            }
            Hand.Add(agent);
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
            _acceptCards.Value = true;
        }

        public void PlaceKing(Coord coord)
        {
            _placeKing.Value = new PlayCard(King.Model, coord);
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

        //private ICard Add(Model.ICard model)
        //{
        //    var card = Arbiter.NewCardAgent(model, this);
        //    Hand.Add(card);
        //    return null;
        //}

        //private void Remove(ICard model)
        //{
        //    var inDeck = Deck.Cards.FirstOrDefault(c => c.Model == model);
        //    Assert.IsNotNull(inDeck);
        //    Deck.Remove(inDeck);
        //    Model.Deck.Remove(Model.Deck.Cards.First(c => c.Template.Id == inDeck.Model.Template.Id));
        //}

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
        private IFuture<ICardAgent> _drawCard;
        private IFuture<PlayCard> _placeKing;
        private IFuture<PlayCard> _playCard;
        private IFuture<MovePiece> _movePiece;
        #endregion
    }
}
