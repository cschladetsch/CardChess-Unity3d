using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flow;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Model
{
    public class Player : ModelBase, IPlayer
    {
        public EColor Color { get; private set; }
        public int Mana { get; set; }
        public int Health { get; set; }
        public IHand Hand { get; }
        public IDeck Deck { get; }
        public IDictionary<CardCollectionDesc, ICardCollection> Collections { get; }

        public static int StartHandCardCount = 7;

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
    }
}
