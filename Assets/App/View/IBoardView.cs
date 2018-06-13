using App.Common;
using App.Model;
using UniRx;
using UnityEngine;

namespace App.View
{
    using Agent;

    public interface IBoardView
        : IView<IBoardAgent>
    {
        IReadOnlyReactiveProperty<ISquareView> HoverSquare { get; }

        ISquareView TestRayCast(Vector3 screen);

        IPieceView PlacePiece(ICardView view, Coord cood);
    }
}
