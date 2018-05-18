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

    public class PlayerModel :
        ModelBase,
        IPlayerModel,
        ICreateWith<EColor>
    {
        #region public Fields
        public EColor Color { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; private set; } = 1;
        public int Health => King.Health;
        public IHandModel HandModel { get; private set; }
        public IDeckModel DeckModel { get; private set; }
        public ICardModel King { get; private set; }
        public IEnumerable<ICardModel> CardsOnBoard { get; }
        public IEnumerable<ICardModel> CardsInGraveyard { get; }
        public static int StartHandCardCount => Parameters.StartHandCardCount;
        #endregion

        #region Public Methods
        public bool Create(EColor color)
        {
            Color = color;
            return true;
        }

        public void SetDeck(IDeckModel deckModel)
        {
            Assert.IsNotNull(deckModel);
            DeckModel = deckModel;
        }

        public Response NewGame()
        {
            MaxMana = 0;
            DeckModel.NewGame();
            HandModel.NewGame();
            King = CardTemplates.New("King", this);
            return Response.Ok;
        }

        public Response ChangeMana(int change)
        {
            Mana = Mathf.Clamp(0, 12, Mana + change);
            return Response.Ok;
        }

        public Response MockMakeHand()
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
            Assert.IsNotNull(DeckModel);
            Assert.IsTrue(DeckModel.NumCards >= 30);
            //TODO HandModel = Arbiter.NewModel<HandModel>(this);
            foreach (var card in DeckModel.Cards.Take((int)Parameters.StartHandCardCount))
            {
                DeckModel.Remove(card as ICardModel);
                HandModel.Add(card as ICardModel);
            }
        }

        public void AddMaxMana(int mana)
        {
            MaxMana = Mathf.Clamp(MaxMana + mana, 0, (int)Parameters.MaxManaCap);
        }
        #endregion
    }
}
