using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Action;

namespace App.Agent.Card
{
    /// <inheritdoc />
    /// <summary>
    /// The most important piece on the board. Can move one square in any direction.
    /// </summary>
    internal class King : CardAgent
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
