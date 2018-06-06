using System;
using System.Collections.Generic;

namespace App.Model
{
    using Common;
    using Common.Message;

    /// <summary>
    /// The NxM playing board. Typically 6x6, 10x10, or 12x12
    /// </summary>
    public interface IBoardModel
        : IModel
        , IGameActor
        , IPrintable
    {
        int Width { get; }
        int Height { get; }
        IArbiterModel Arbiter { get; set; }
        IEnumerable<IPieceModel> Pieces { get; }

        bool IsValidCoord(Coord coord);
        string CardToRep(ICardModel cardModel);
        int NumPieces(EPieceType type);
        string Print(Func<Coord, string> fun);

        Response Remove(IPieceModel pieceModel);
        Response<IPieceModel> TryPlacePiece(PlacePiece placePiece);
        Response TryMovePiece(MovePiece act);

        IEnumerable<IPieceModel> PiecesOfType(EPieceType type);
        IPieceModel At(int x, int y);
        IPieceModel At(Coord coord);
        IEnumerable<IPieceModel> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<IPieceModel> AttackedCards(Coord cood);
        IEnumerable<IPieceModel> DefendededCards(IPieceModel defender, Coord cood);
        IEnumerable<IPieceModel> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);
    }
}
