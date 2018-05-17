using System.Collections.Generic;
using App.Action;

namespace App.Agent.Card
{
    /// <inheritdoc />
    /// <summary>
    /// The strongest piece on the Board. Can move any number of squares diagonaly or orthogonally.
    /// </summary>
    internal class Queen : CardAgent
    {
        /// <inheritdoc />
        public override IEnumerable<Coord> PotentialCoords()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICard> Attackers(Coord coord)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICard> Defenders(Coord coord)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICard> Guards(Coord coord)
        {
            throw new System.NotImplementedException();
        }
    }
}
