namespace App.Model
{
    using System.Collections.Generic;
    using Common;

    /// <summary>
    /// Used to return move request results for a square or piece
    /// </summary>
    public class MoveResults
    {
        /// <summary>
        /// Valid movement squares
        /// </summary>
        public readonly List<Coord> Coords = new List<Coord>();
        
        /// <summary>
        /// Pieces that are 'in the way' of the movement.
        /// </summary>
        public readonly List<IPieceModel> Interrupts = new List<IPieceModel>();
    }
}
