namespace App.Common.Message
{
    using Model;
    using Common;

    /// <summary>
    /// An attempt to move a piece on the board.
    /// May have sequences, like starting a Battle.
    /// </summary>
    public class MovePiece : RequestBase
    {
        public IPieceModel Piece;
        public Coord Coord;
    }
}
