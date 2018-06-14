using App.Common;
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

        ISquareView TestRayCast(Vector3 screen);
        IPieceView PlacePiece(ICardView view, Coord cood);
    }
}
