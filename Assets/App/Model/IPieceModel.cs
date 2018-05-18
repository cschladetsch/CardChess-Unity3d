using App.Action;
using App.Common;

namespace App.Model.Card
{
    public interface IPieceModel
        : ICardModel
    {
        Coord Coord { get; }
        IBoardModel Board { get; }
    }
}
