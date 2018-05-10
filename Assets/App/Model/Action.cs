
namespace App.Model
{
    /// <summary>
    /// Move a piece on the board
    /// </summary>
    public class MovePiece : ActionBase
    {
        public IInstance Instance;
        public Coord Target;
    }

    /// <summary>
    /// Pass the turn
    /// </summary>
    public class Pass : ActionBase
    {
    }
}
