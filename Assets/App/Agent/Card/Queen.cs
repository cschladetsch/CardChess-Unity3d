using System.Collections.Generic;
using App.Action;

namespace App.Agent.Card
{
    /// <inheritdoc />
    /// <summary>
    /// The strongest piece on the Board.
    /// </summary>
    internal class Queen : PieceInstance
    {
        /// <inheritdoc />
        public override IEnumerable<Coord> PotentialCoords()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<IPieceInstance> Attackers(Coord coord)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<IPieceInstance> Defenders(Coord coord)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<IPieceInstance> Guards(Coord coord)
        {
            throw new System.NotImplementedException();
        }
    }
}
