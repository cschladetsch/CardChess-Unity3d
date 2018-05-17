﻿using System;
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

    public class Player :
        ModelBase,
        IPlayer,
        ICreateWith<EColor>
    {
        #region public Fields
        public EColor Color { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; private set; } = 1;
        public int Health => King.Health;
        public IHand Hand { get; private set; }
        public IDeck Deck { get; private set; }
        public ICardInstance King { get; private set; }
        public IEnumerable<ICardInstance> CardsOnBoard { get; }
        public IEnumerable<ICardInstance> CardsInGraveyard { get; }
        public static int StartHandCardCount => Parameters.StartHandCardCount;
        #endregion

        #region Public Methods
        public bool Create(EColor color)
        {
            Color = color;
            return true;
        }

        public void SetDeck(IDeck deck)
        {
            Assert.IsNotNull(deck);
            Deck = deck;
        }

        public void NewGame()
        {
            MaxMana = 0;
            Deck.NewGame();
            Hand.NewGame();
            King = CardTemplates.New("King", this);
        }

        public void ChangeMana(int change)
        {
            Mana = Mathf.Clamp(0, 12, Mana + change);
        }

        public void ChangeMaxMana(int change)
        {
            MaxMana = Mathf.Clamp(0, 12, Mana + change);
        }

        public void DrawHand()
        {
            Assert.IsNotNull(Deck);
            Assert.IsTrue(Deck.Cards.Count >= 30);
            Hand = Arbiter.NewModel<Hand>(this);
            foreach (var card in Deck.Cards.Take(Parameters.StartHandCardCount))
            {
                Deck.Remove(card);
                Hand.Add(card);
            }
        }

        public void AddMaxMana(int mana)
        {
            MaxMana = Mathf.Clamp(MaxMana + mana, 0, Parameters.MaxManaCap);
        }
        #endregion
    }
}