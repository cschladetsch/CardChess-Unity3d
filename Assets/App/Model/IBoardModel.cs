using System;
using System.Collections.Generic;
using App.Registry;
using UniRx;

namespace App.Model
{
    using Common;
    using Common.Message;

    /// <summary>
    /// The NxM playing board. Typically 6x6, 10x10, or 12x12
    /// </summary>
    public interface IBoardModel
        : IModel
        , IPrintable
    {
        #region Properties
        int Width { get; }
        int Height { get; }
        [Inject] IArbiterModel Arbiter { get; set; }
        IReadOnlyReactiveCollection<IPieceModel> Pieces { get; }
        #endregion

        #region Methods
        void NewGame();
        bool IsValidCoord(Coord coord);
        string CardToRep(ICardModel cardModel);
        int NumPieces(EPieceType type);
        string Print(Func<Coord, string> fun);

        Response Remove(IPieceModel pieceModel);
        Response<IPieceModel> TryPlacePiece(PlacePiece placePiece);
        Response TryMovePiece(MovePiece act);

        IEnumerable<IPieceModel> GetPiecesOfType(EPieceType type);
        IPieceModel At(int x, int y);
        IPieceModel At(Coord coord);
        IEnumerable<IPieceModel> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<IPieceModel> AttackedCards(Coord cood);
        IEnumerable<IPieceModel> DefendededCards(IPieceModel defender, Coord cood);
        IEnumerable<IPieceModel> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);
        #endregion
    }
}
