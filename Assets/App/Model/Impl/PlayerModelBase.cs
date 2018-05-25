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

    public abstract class PlayerModelBase
        : ModelBase
        , IPlayerModel
    {
        #region public Fields
        public EColor Color { get; }
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;
        public bool AcceptedHand { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; private set; } = 1;
        public int Health => King.Health;
        [Inject] public IBoardModel Board { get; set; }
        [Inject] public IArbiterModel Arbiter { get; set; }
        [Inject] public Service.ICardTemplateService _cardtemplateService;
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

        public override string ToString()
        {
            return $"{Color}";
        }

        public PlayerModelBase(EColor color)
        {
            Color = color;
            Owner = this;
        }

        public virtual Response NewGame()
        {
            AcceptedHand = false;
            MaxMana = 0;
            // TODO: pass Guid of a pre-built player deck
            Deck = Registry.New<IDeckModel>(Guid.Empty, this);
            Hand = Registry.New<IHandModel>(this);
            Deck.NewGame();
            Hand.NewGame();
            var tmpl = Database.CardTemplates.OfType(EPieceType.King).First();
            King = Registry.New<ICardModel>(tmpl, this);
            return Response.Ok;
        }

        public void CardExhaustion()
        {
            King.ChangeHealth(-1, null);
        }

        public Response CardDrawn(ICardModel card)
        {
            if (Hand.NumCards == Hand.MaxCards)
                return Response.Fail;
            return Hand.Add(card) ? Response.Ok : Response.Fail;
        }

        public virtual void RequestFailed(IRequest req)
        {
            Warn($"{req} Failed");
        }

        public virtual void RequestSuccess(IRequest req)
        {
            Verbose(10, $"{req} succeeded");
        }

        public abstract IRequest Mulligan();
        public abstract IRequest StartTurn();
        public abstract IRequest NextAction();

        public Response ChangeMana(int change)
        {
            Mana = Mathf.Max(0, 12, Mana + change);
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
