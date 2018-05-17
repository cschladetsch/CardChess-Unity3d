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
        public override IEnumerable<ICard> Attackers(Coord coord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICard> Defenders(Coord coord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICard> Guards(Coord coord)
        {
            throw new NotImplementedException();
        }
    }
}
