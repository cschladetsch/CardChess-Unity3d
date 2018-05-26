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
        , IPrintable
    {
        #region Properties
        int Width { get; }
        int Height { get; }
        IArbiterModel Arbiter { get; }
        IEnumerable<IPieceModel> Pieces { get; }
        #endregion

        #region Methods
        string Print(Func<Coord, string> fun);
        void NewGame();
        bool IsValidCoord(Coord coord);
        bool TryPlacePiece(IPieceModel piece, Coord coord);
        string CardToRep(ICardModel cardModel);
        int NumPieces(EPieceType type);
        Response TryMovePiece(IPieceModel piece, Coord coord);
        IEnumerable<IPieceModel> GetContents();
        IEnumerable<IPieceModel> GetPieces(EPieceType type);
        IPieceModel GetContents(Coord coord);
        IPieceModel At(Coord coord);
        IEnumerable<IPieceModel> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<IPieceModel> AttackedCards(Coord cood);
        IEnumerable<IPieceModel> DefendededCards(IPieceModel defender, Coord cood);
        IEnumerable<IPieceModel> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);
        #endregion
    }
}
