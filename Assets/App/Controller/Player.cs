using System;
using Flow;

using App.Model;

namespace App.Controller
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

        public IFuture<Model.PlayCard> TryPlayCard()
        {
            throw new NotImplementedException();
        }

        public IFuture<Model.MovePiece> TryMovePiece()
        {
            throw new NotImplementedException();
        }

        public IFuture<bool> Pass()
        {
            throw new NotImplementedException();
        }
    }
}