using System.Collections.Generic;
using App.Common;
using App.Common.Message;
using App.Model;
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
        , IPrintable
    {
        IReadOnlyReactiveProperty<int> Width { get; }
        IReadOnlyReactiveProperty<int> Height { get; }
        IReadOnlyReactiveProperty<ISquareView> HoverSquare { get; }
        IReadOnlyReactiveCollection<IPieceView> Pieces { get; }

        Material BlackMaterial { get; }
        Material WhiteMaterial { get; }

        IPieceView Get(Coord coord);
        //IPieceView PlacePiece(ICardView view, Coord cood);

        IResponse Remove(IPieceView pieceView);
        IResponse MovePiece(IPieceView pieceView, Coord coord);

        ISquareView TestRayCast(Vector3 screen);
        void ShowSquares(ICardModel cardView, ISquareView sq);
    }
}
