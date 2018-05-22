using System;
using System.Collections.Generic;
using System.Linq;
using App.Action;
using App.Database;
using UnityEngine;
using UnityEngine.Assertions;

// Reflection deals with this
// ReSharper disable PublicConstructorInAbstractClass

namespace App.Model
{
    using Common;
    using Registry;

    public /*abstract*/ class PlayerModel
        : ModelBase
        , IPlayerModel
    {
        #region public Fields
        public EColor Color { get; }
        public bool AcceptedHand { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; private set; } = 1;
        public int Health => King.Health;
        [Inject] public IBoardModel Board { get; set; }
        [Inject] public IArbiterModel Arbiter { get; set; }
        public IHandModel Hand { get; private set; }
        public IDeckModel Deck { get; private set; }
        public ICardModel King { get; private set; }

        public IEnumerable<ICardModel> CardsOnBoard =>
            Board.Pieces.
            Where(p => p.Owner == this).
            Select(p => p.Card);
        public IEnumerable<ICardModel> CardsInGraveyard { get; }
        #endregion

        #region Public Methods

        public PlayerModel(EColor color)
        {
            Color = color;
        }

        public Response NewGame()
        {
            AcceptedHand = false;
            MaxMana = 0;
            // TODO: pass Guid of a pre-built player deck
            Deck = Registry.New<IDeckModel>(Guid.Empty, this);
            Hand = Registry.New<IHandModel>(this);
            Deck.NewGame();
            Hand.NewGame();
            King = CardTemplates.NewCardModel(Registry, "King", this);
            return Response.Ok;
        }

        public void CardExhaustion()
        {
            King.ChangeHealth(-1, null);
        }

        public virtual IAction NextAction()
        {
            switch (Arbiter.GameState)
            {
                case EGameState.None:
                    break;
                case EGameState.Shuffling:
                    Deck.Shuffle();
                    break;
                case EGameState.Dealing:
                    DrawHand();
                    break;
                case EGameState.Mulligan:
                    return Mulligan();
                case EGameState.Ready:
                    break;
                case EGameState.PlaceKing:
                    return PlaceKing();
                case EGameState.TurnStart:
                    Hand.Add(Deck.Draw());
                    break;
                case EGameState.TurnPlay:
                    return DecideTurn();
                case EGameState.Battle:
                    break;
                case EGameState.TurnEnd:
                    break;
                case EGameState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Action.Pass(this);
        }

        // TODO: make MockPlayer that implements these
        //protected abstract IAction Mulligan();
        //protected abstract IAction PlaceKing();
        //protected abstract IAction DecideTurn();

        protected IAction Mulligan()
        {
            return null;
        }
        protected IAction PlaceKing()
        {
            return null;
        }

        protected IAction DecideTurn()
        {
            return null;
        }

        public Response ChangeMana(int change)
        {
            Mana = Mathf.Clamp(0, 12, Mana + change);
            return Response.Ok;
        }

        public Response DrawHand()
        {
            Hand.Add(Deck.Draw(Parameters.StartHandCardCount));
            return Response.Ok;
        }

        public Response ChangeMaxMana(int change)
        {
            MaxMana = Mathf.Clamp(0, 12, Mana + change);
            return Response.Ok;
        }

        public void AddMaxMana(int mana)
        {
            MaxMana = Mathf.Clamp(MaxMana + mana, 0, (int)Parameters.MaxManaCap);
        }
        #endregion
    }
}
