using System;
using Flow;

namespace App
{
    public class Player : Agent, IPlayer
    {
        public IUser User { get; private set; }
        public Model.Hand Hand { get; private set; }
        public EColor Color { get; private set; }
        public IFuture<int> RollDice()
        {
            throw new NotImplementedException();
        }

        public void AddMaxMana(int mana)
        {
            throw new NotImplementedException();
        }

        public IFuture<Arbiter.PlayCard> TryPlayCard()
        {
            throw new NotImplementedException();
        }

        public IFuture<Arbiter.MovePiece> TryMovePiece()
        {
            throw new NotImplementedException();
        }

        public IFuture<bool> Pass()
        {
            throw new NotImplementedException();
        }
    }
}