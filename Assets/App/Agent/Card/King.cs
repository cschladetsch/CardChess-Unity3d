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
    /// The most important piece on the board.
    /// </summary>
    internal class King : PieceInstance
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
