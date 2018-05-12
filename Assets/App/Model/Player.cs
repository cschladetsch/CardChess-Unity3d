using System.Collections.Generic;
using System.Linq;
using Flow;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Model
{
    public class Player : ModelBase, IPlayer
    {
        public EColor Color { get; private set; }
        public int MaxMana { get; private set; }
        public int Mana { get; private set; } = 1;
        public int Health => King.Health;
        public IHand Hand { get; private set; }
        public IDeck Deck { get; private set; }
        public ICardInstance King { get; private set; }
        public IEnumerable<ICardInstance> CardsOnBoard { get; }
        public IEnumerable<ICardInstance> CardsInGraveyard { get; }

        public static int StartHandCardCount = 7;

        public Player() { }

        public bool Create(EColor a0, IDeck deck)
        {
            Color = a0;
            MaxMana = 1;
            King = Database.CardTemplates.New("King", this);
            Hand = MockMakeHand();
            Deck = deck;
            return true;
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

        public void NewGame()
        {
            Deck.NewGame();
            Hand.NewGame();

            Assert.AreEqual(Hand.Cards.Count, 0);
            Assert.IsTrue(Deck.Cards.Count >= 7);

            foreach (var card in Deck.Cards.Take(7))
            {
                Hand.Add(card);
            }

            // TODO: sort out Agent/Model
            //Hand.Add(NewKingCard());
        }

        public Player(EColor color)
        {
            Color = color;
        }

        public void AddMaxMana(int mana)
        {
            throw new System.NotImplementedException();
        }

        public IFuture<PlayCard> TryPlayCard()
        {
            throw new System.NotImplementedException();
        }

        public IFuture<MovePiece> TryMovePiece()
        {
            throw new System.NotImplementedException();
        }

        public IFuture<bool> Pass()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeMaxMana(int change)
        {
            MaxMana = Mathf.Clamp(0, 12, Mana + change);
        }

    }
}
