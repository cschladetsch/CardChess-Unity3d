namespace App.Action
{
    /// <inheritdoc />
    /// <summary>
    /// Move a piece on the board
    /// </summary>
    public class MovePiece : ActionBase
    {
        public Agent.ICardInstance Instance;
        public Coord Target;
    }
}