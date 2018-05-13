using System;
using System.Collections.Generic;
using System.Linq;
using App.Database;
using Flow;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Model
{
    using App.Action;

    public class Player : ModelBase, IPlayer, ICreateWith<EColor>
    {
        public static int StartHandCardCount = 7;

        public EColor Color { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; private set; } = 1;
        public int Health => King.Health;
        public IHand Hand { get; private set; }
        public IDeck Deck { get; private set; }
        public ICardInstance King { get; private set; }
        public IEnumerable<ICardInstance> CardsOnBoard { get; }
        public IEnumerable<ICardInstance> CardsInGraveyard { get; }

        public Player() { }

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
            MaxMana = 1;
            King = CardTemplates.New("King", this);
            Deck.NewGame();

            int cardsInDeck = Deck.Cards.Count;
            Hand = MockMakeHand();

            Assert.AreEqual(Hand.Cards.Count, 7);
            Assert.IsTrue(Deck.Cards.Count == cardsInDeck - 7);
        }

        public void ChangeMana(int change, Action<EResponse> response)
        {
            Mana = Mathf.Clamp(0, 12, Mana + change);
            response(EResponse.Ok);
        }

        public void ChangeMaxMana(int change, Action<EResponse> response)
        {
            MaxMana = Mathf.Clamp(0, 12, Mana + change);
            response(EResponse.Ok);
        }

        private IHand MockMakeHand()
        {
            Assert.IsNotNull(Deck);
            Info("{0} cards in Deck", Deck.Cards.Count);
            Assert.IsTrue(Deck.Cards.Count >= 30);
            var hand = new Hand();
            foreach (var card in Deck.Cards.Take(7))
            {
                Info("Adding {0}", card.Template.Name);
                Deck.Remove(card);
                hand.Add(card);
            }
            return hand;
        }

        public void AddMaxMana(int mana)
        {
            throw new NotImplementedException();
        }

        public IFuture<PlayCard> TryPlayCard()
        {
            throw new NotImplementedException();
        }

        public IFuture<MovePiece> TryMovePiece()
        {
            throw new NotImplementedException();
        }

        public IFuture<bool> Pass()
        {
            throw new NotImplementedException();
        }
    }
}
