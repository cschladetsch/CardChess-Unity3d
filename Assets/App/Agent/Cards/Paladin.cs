using System;
using System.Collections.Generic;
using App.Action;
using App.Agent;

namespace App.Agent.Card
{
    /// <inheritdoc />
    /// <summary>
    /// Paladins are like the Pawns of Chess.
    /// </summary>
    internal class Paladin : CardAgent
    {
        /// <inheritdoc />
        public override IEnumerable<Coord> PotentialCoords()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICardAgent> Attackers(Coord coord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICardAgent> Defenders(Coord coord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICardAgent> Guards(Coord coord)
        {
            throw new NotImplementedException();
        }
    }
}
