using System.Collections.Generic;
using App.Action;

namespace App.Agent.Card
{
    /// <inheritdoc />
    /// <summary>
    /// The strongest piece on the BoardAgent. Can move any number of squares diagonaly or orthogonally.
    /// </summary>
    internal class Queen : CardAgent
    {
        /// <inheritdoc />
        public override IEnumerable<Coord> PotentialCoords()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICardAgent> Attackers(Coord coord)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICardAgent> Defenders(Coord coord)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<ICardAgent> Guards(Coord coord)
        {
            throw new System.NotImplementedException();
        }
    }
}
