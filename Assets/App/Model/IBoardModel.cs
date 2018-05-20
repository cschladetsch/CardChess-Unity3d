using System;
using System.Collections.Generic;
using System.Text;
using App.Model;

namespace App.Model
{
    using Action;
    using Common;

    /// <summary>
    /// The NxN playing board Typically 6x6, 10x10, or 12x12 (Desktop only)
    /// </summary>
    public interface IBoardModel
        : IModel
        , IPrintable
    {
        #region Properties
        int Width { get; }
        int Height { get; }
        IArbiterModel Arbiter { get; }
        IPlayerModel WhitePlayer { get; }
        IPlayerModel BlackPlayer { get; }
        #endregion

        #region Methods
        void NewGame(IArbiterModel arbiter);
        IEnumerable<IPieceModel> GetContents();
        IPieceModel GetContents(Coord coord);
        IPieceModel At(Coord coord);
        bool IsValidCoord(Coord coord);
        IPieceModel PlaceCard(ICardModel cardModel, Coord coord);
        bool CanPlaceCard(ICardModel card, Coord coord);
        IEnumerable<IPieceModel> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<IPieceModel> AttackedCards(Coord cood);
        IEnumerable<IPieceModel> DefendededCards(IPieceModel defender, Coord cood);
        IEnumerable<IPieceModel> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);

        string Print(Func<Coord, string> fun);
        string CardToRep(ICardModel cardModel);
        #endregion
    }
}
