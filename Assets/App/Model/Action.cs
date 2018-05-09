
namespace App.Model
{
    public enum EAction
    {
        None,
        Pass,
        PlayCard,
        MovePiece,
        ClaimDraw,
        ClaimWin,
    }

    public interface IAction
    {
        IPlayer Player { get; }
        EAction Action { get; }
    }

    /// <summary>
    /// Common to all Actions
    /// </summary>
    public abstract class ActionBase : IAction
    {
        public IPlayer Player { get; protected set; }
        public EAction Action { get; protected set; }
    }

    /// <summary>
    /// Play a card from a Player's Hand onto the Board
    /// </summary>
    public class PlayCard : ActionBase
    {
        public ICard Card;
        public Coord Coord;
    }

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
