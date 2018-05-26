namespace App.Common.Message
{
    using Model;
    using Common;

    /// <summary>
    /// An attempt to move a piece on the board.
    /// The target location must be empty. If a battle is required,
    /// create a Battle request instead. The same for mounting.
    /// </summary>
    public class MovePiece : RequestBase
    {
        public IPieceModel Piece;
        public Coord Coord;

        public MovePiece(IPlayerModel player, IPieceModel piece, Coord coord)
            : base(player, EActionType.MovePiece)
        {
            Piece = piece;
            Coord = coord;
        }

        public override string ToString()
        {
            return $"{Player} {Piece} move to {Coord}";
        }
    }
}
