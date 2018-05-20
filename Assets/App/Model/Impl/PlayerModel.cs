using System;
using System.Collections.Generic;
using System.Linq;
using App.Database;
using Flow;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Model
{
    using Action;
    using Common;

    public class PlayerModel
        : ModelBase
        , IPlayerModel
    {
        #region public Fields
        public EColor Color { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; private set; } = 1;
        public int Health => King.Health;
        [Inject] public IBoardModel Board { get; }
        [Inject] public IArbiterModel Arbiter { get; }
        public IHandModel Hand { get; private set; }
        public IDeckModel Deck { get; private set; }
        public ICardModel King { get; private set; }
        public IEnumerable<ICardModel> CardsOnBoard { get; }
        public IEnumerable<ICardModel> CardsInGraveyard { get; }
        public static int StartHandCardCount => Parameters.StartHandCardCount;
        #endregion

        public PlayerModel(EColor color)
        {
            Color = color;
        }

        #region Public Methods

        public void SetDeck(IDeckModel deckModel)
        {
            Assert.IsNotNull(deckModel);
            Deck = deckModel;
        }

        public Response NewGame()
        {
            MaxMana = 0;
            Deck.NewGame();
            Hand.NewGame();
            King = CardTemplates.NewCardModel(Registry, "King", this);
            return Response.Ok;
        }

        public Response ChangeMana(int change)
        {
            Mana = Mathf.Clamp(0, 12, Mana + change);
            return Response.Ok;
        }

        Response IPlayerModel.DrawHand()
        {
            throw new NotImplementedException();
        }

        public Response AcceptHand()
        {
            throw new NotImplementedException();
        }

        public Response PlayCard(ICardModel model, Coord coord)
        {
            throw new NotImplementedException();
        }

        public Response MovePiece(ICardModel model, Coord coord)
        {
            throw new NotImplementedException();
        }

        public Response Pass()
        {
            throw new NotImplementedException();
        }

        public Response ChangeMaxMana(int change)
        {
            MaxMana = Mathf.Clamp(0, 12, Mana + change);
            return Response.Ok;
        }

        public void DrawHand()
        {
            Assert.IsNotNull(Deck);
            Assert.IsTrue(Deck.NumCards >= 30);
            //TODO Hand = Arbiter.NewModel<Hand>(this);
            foreach (var card in Deck.Cards.Take((int)Parameters.StartHandCardCount))
            {
                Deck.Remove(card as ICardModel);
                Hand.Add(card as ICardModel);
            }
        }

        public void AddMaxMana(int mana)
        {
            MaxMana = Mathf.Clamp(MaxMana + mana, 0, (int)Parameters.MaxManaCap);
        }
        #endregion
    }
}
