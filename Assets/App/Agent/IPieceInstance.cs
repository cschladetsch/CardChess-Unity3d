using App.Action;
using App.Model;
using Flow;

namespace App.Agent
{
    public interface IPieceInstance : ICardInstance, IOwned
    {
        Coord Coord { get; }
        ECardType Type { get; }
        IFuture<ModePiece> Move();
    }
}
