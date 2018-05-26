using System;
using System.Collections.Generic;
using Flow;

namespace App.Model
{
    using Common;
    using Registry;

    /// <summary>
    /// <summary>
    /// Model of a piece on the board.
    /// </summary>
    public class PieceModel
        : PlayerOwnedModelBase
        , IPieceModel
    {
        public ICardModel Card { get; private set; }
        public EPieceType Type => Card.PieceType;
        [Inject] public IBoardModel Board { get; set; }
        public Coord Coord { get; private set; }
        public int Power => Card.Power;
        public int Health => Card.Health;

        public PieceModel(IPlayerModel a0, ICardModel a1)
        {
            Construct(a0, a1);
        }

        public bool Construct(IPlayerModel a0, ICardModel a1)
        {
            base.Construct(a0);
            Card = a1;
            return true;
        }

        public void Attack(IPieceModel defender)
        {
            throw new NotImplementedException();
        }
    }
}
