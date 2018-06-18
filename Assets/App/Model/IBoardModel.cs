using System;
using System.Collections.Generic;
using App.Agent;
using Flow;
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
        int Width { get; set; }
        int Height { get; set; }
        IArbiterModel Arbiter { get; set; }

        IReadOnlyReactiveDictionary<Coord, IPieceModel> Pieces { get; }

        bool IsValidCoord(Coord coord);
        string CardToRep(ICardModel cardModel);
        int NumPieces(EPieceType type);
        string Print(Func<Coord, string> fun);

        IResponse<IPieceModel> TryPlacePiece(PlacePiece place);
        IResponse TryMovePiece(MovePiece act);

        IEnumerable<IPieceModel> PiecesOfType(EPieceType type);
        IPieceModel At(int x, int y);
        IPieceModel At(Coord coord);

        IEnumerable<IPieceModel> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<IPieceModel> AttackedCards(Coord cood);
        IEnumerable<IPieceModel> DefendededCards(IPieceModel defender, Coord cood);
        IEnumerable<IPieceModel> Defenders(Coord cood);

        IEnumerable<Coord> GetMovements(Coord cood);
        IEnumerable<Coord> GetAttacks(Coord cood);

        IEnumerable<Coord> GetMovements(Coord cood, EPieceType type);
        IEnumerable<Coord> GetAttacks(Coord cood, EPieceType type);

        // directly change model state
        IResponse Add(IPieceModel model);
        IResponse Remove(IPieceModel pieceModel);
        IResponse Move(IPieceModel pieceModel, Coord coord);
        void NewTurn();
    }
}
