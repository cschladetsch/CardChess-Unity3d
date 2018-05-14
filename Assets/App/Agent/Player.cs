using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flow;

namespace App.Agent
{
    using Action;

    /// <inheritdoc cref="IPlayer" />
    /// <summary>
    /// The agent that represents a player in the game.
    /// </summary>
    public class Player : AgentBaseCoro<Model.IPlayer>, IPlayer
    {
        public EColor Color => Model.Color;
        public ICardInstance King { get; private set; }
        public int Health => King.Health;

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

        public IFuture<EResponse> NewGame()
        {
            Model.NewGame();
            King = Arbiter.NewAgent<CardInstance, Model.ICardInstance>(Model.King);
            var future = New.Future<EResponse>();
            future.Value = EResponse.Ok;
            return future;
        }

        public ITransient StartGame()
        {
            return null;
        }

        public IGenerator DrawInitialCards()
        {
            foreach (var card in Model.Deck.Cards.Take(App.Model.Player.StartHandCardCount))
            {
                Model.Hand.Cards.Add(card);
                Model.Deck.Cards.Remove(card);
            }

            return null;
        }

        public void RedrawCards(params Guid[] rejected)
        {
            throw new NotImplementedException();
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

        public IFuture<bool> HasAcceptedCards()
        {
            return _hasAccepted = New.NamedFuture<bool>("HasAcceptedCards");
        }

        public void AcceptCards()
        {
            _hasAccepted.Complete();
        }

        public IFuture<PlayCard> HasPlacedKing()
        {
            return _hasPlacedKing = New.NamedFuture<PlayCard>("HasPlacedKing");
        }

        public void PlaceKing(Coord coord)
        {
            if (App.Main.Arbiter.CanPlaceKing(this, coord))
                _hasPlacedKing.Value = new PlayCard(King, coord);
        }

        public void AcceptKingPlacement()
        {
            // Unused?
        }

        public ITransient DeliverCards()
        {
            // TODO: animate
            return null;
        }

        public ITransient DrawCard()
        {
            var card = Model.Deck.Cards.First();
            Model.Hand.Add(card);
            Model.Deck.Cards.Remove(card);
            return null;
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
        private IFuture<bool> _hasAccepted;
        private IFuture<PlayCard> _hasPlacedKing;
    }
}
