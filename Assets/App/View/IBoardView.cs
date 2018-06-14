using App.Common;
using App.View.Impl1;
using UniRx;
using UnityEngine;

namespace App.View
{
    using Agent;

    /// <summary>
    /// View of the Board in the scene.
    /// </summary>
    public interface IBoardView
        : IView<IBoardAgent>
    {
        IReadOnlyReactiveProperty<int> Width { get; }
        IReadOnlyReactiveProperty<int> Height { get; }
        IReadOnlyReactiveProperty<ISquareView> HoverSquare { get; }

        Material BlackMaterial { get; }
        Material WhiteMaterial { get; }

        IPieceView Get(Coord coord);
        ISquareView TestRayCast(Vector3 screen);
        IPieceView PlacePiece(ICardView view, Coord cood);
        void ShowSquares(PieceView pieceView);
    }
}
