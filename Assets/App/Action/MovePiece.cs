namespace App.Action
{
    /// <inheritdoc />
    /// <summary>
    /// A valid description of a movement of a piece on the Board.
    /// </summary>
    public class MovePiece : ActionBase
    {
        public Agent.ICard Instance;
        public Coord Target;
    }
}
