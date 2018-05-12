
namespace App.Action
{
    /// <summary>
    /// Move a piece on the board
    /// </summary>
    public class MovePiece : ActionBase
    {
        public Agent.ICardInstance Instance;
        public Coord Target;
    }

    /// <summary>
    /// Pass the turn
    /// </summary>
    public class Pass : ActionBase
    {
    }
}
