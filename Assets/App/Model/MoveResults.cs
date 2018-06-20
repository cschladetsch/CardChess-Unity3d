using System.Collections.Generic;
using App.Common;

namespace App.Model
{
    /// <summary>
    /// Used to return move request results for a square or piece
    /// </summary>
    public class MoveResults
    {
        /// <summary>
        /// Pieces that are 'in the way' of the movemenet.
        /// </summary>
        public List<IPieceModel> Interferernce = new List<IPieceModel>();

        /// <summary>
        /// Valid movement squares
        /// </summary>
        public List<Coord> Coords = new List<Coord>();
    }
}
