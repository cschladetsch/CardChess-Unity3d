using System;
using System.Collections.Generic;
using Flow;

namespace App.Model
{
	using Common;
	using Registry;

    /// <summary>
    /// Model of a piece on the board.
    /// </summary>
    public class PieceModel
        : PlayerOwnedModelBase
        , IPieceModel
    {
		public ICardModel Card { get; private set; }
        public EPieceType Type => Card.PieceType;
        [Inject] public IBoardModel Board { get; }
        public Coord Coord { get; }
        public int Damage { get; }
        public int Health { get; }
        public bool Alive { get; }

        public bool Construct(IPlayerModel a0, ICardModel a1)
        {
			base.Construct(a0);
			Card = a1;
			return true;
        }

        public IGenerator Battle(ICardModel other)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Attacking()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Defending()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Attackers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Defenders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Neareby(int distance)
        {
            throw new NotImplementedException();
        }

        public void Respond(IPieceModel attacker)
        {
            throw new NotImplementedException();
        }

        public void Attack(IPieceModel defender)
        {
            throw new NotImplementedException();
        }
    }
}
