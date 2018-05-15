using App.Action;
using App.Model;
using Flow;

namespace App.Agent
{
    /// <summary>
    /// An active piece on the Board.
    /// </summary>
    public interface IPieceInstance :
        ICardInstance,
        IOwned
    {
        Coord Coord { get; }
        ECardType Type { get; }
        IFuture<MovePiece> Move();
    }
}
