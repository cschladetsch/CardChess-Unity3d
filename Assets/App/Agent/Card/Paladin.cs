using System;
using System.Collections.Generic;
using App.Action;
using App.Agent;

namespace Assets.App.Agent.Card
{
    /// <inheritdoc />
    /// <summary>
    /// Paladins are like the Pawns of Chess.
    /// </summary>
    internal class Paladin : PieceAgent
    {
        /// <inheritdoc />
        public override IEnumerable<Coord> PotentialCoords()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<IPieceInstance> Attackers(Coord coord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<IPieceInstance> Defenders(Coord coord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<IPieceInstance> Guards(Coord coord)
        {
            throw new NotImplementedException();
        }
    }
}
