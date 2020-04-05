namespace App.Model
{
    using System;
    using System.Collections.Generic;
    using UniRx;
    using Dekuple;
    using Dekuple.Model;
    using Common;
    using Common.Message;

    /// <inheritdoc cref="IModel" />
    /// <summary>
    /// The NxM playing board. Typically 6x6, 10x10, or 12x12
    /// </summary>
    public interface IBoardModel
        : IModel
        , IPrintable
        , IGameActor
    {
        int Width { get; set; }
        int Height { get; set; }
        IArbiterModel Arbiter { get; set; }
        IReadOnlyReactiveCollection<IPieceModel> Pieces { get; }

        bool IsValidCoord(Coord coord);
        string CardToRep(ICardModel cardModel);
        int NumPieces(EPieceType type);
        string Print(Func<Coord, string> fun);

        IResponse<IPieceModel> TryPlacePiece(PlacePiece place);
        IResponse TryMovePiece(MovePiece act);

        IEnumerable<IPieceModel> PiecesOfType(EPieceType type);
        IPieceModel At(int x, int y);
        IPieceModel At(Coord coord);

        MoveResults GetMovements(IPieceModel piece);
        MoveResults GetMovements(Coord coord, EPieceType type);

        MoveResults GetAttacks(IPieceModel piece);
        MoveResults GetAttacks(Coord coord, EPieceType type);
        MoveResults GetAdjacent(Coord coord, int distance);

        IResponse Add(IPieceModel model);
        IResponse Remove(IPieceModel pieceModel);
        IResponse Move(IPieceModel pieceModel, Coord coord);

        void NewTurn();
        bool CanMoveOrAttack(IPieceModel pieceModel);
    }
}
