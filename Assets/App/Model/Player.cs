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

    public class Player : ModelBase, IPlayer
    {
        public static int StartHandCardCount = 7;

        public Player()
        {
        }

        public Player(EColor color)
        {
            Color = color;
        }

        public EColor Color { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; } = 1;
        public int Health => King.Health;
        public IHand Hand { get; private set; }
        public IDeck Deck { get; private set; }
        public ICardInstance King { get; private set; }
        public IEnumerable<ICardInstance> CardsOnBoard { get; }
        public IEnumerable<ICardInstance> CardsInGraveyard { get; }

        public bool Create(EColor a0, IDeck deck)
        {
            Color = a0;
            MaxMana = 1;
            King = CardTemplates.New("King", this);
            Hand = MockMakeHand();
            Deck = deck;
            return true;
        }

        public void NewGame()
        {
            Deck.NewGame();
            Hand.NewGame();

            Assert.AreEqual(Hand.Cards.Count, 0);
            Assert.IsTrue(Deck.Cards.Count >= 7);

            foreach (var card in Deck.Cards.Take(7))
                Hand.Add(card);

            // TODO: sort out Agent/Model
            //Hand.Add(NewKingCard());
        }

        public void ChangeMaxMana(int change)
        {
            MaxMana = Mathf.Clamp(0, 12, Mana + change);
        }

        private IHand MockMakeHand()
        {
            Assert.IsNotNull(Deck);
            Assert.IsTrue(Deck.Cards.Count >= 30);
            var hand = new Hand();
            foreach (var card in Deck.Cards.Take(7))
            {
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
