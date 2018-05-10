using System.Collections;
using System.Collections.Generic;
using Flow;
using UnityEngine;

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
