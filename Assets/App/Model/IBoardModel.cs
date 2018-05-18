using System;
using System.Collections.Generic;
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
        //, ICreateWith<int, int>
    {
        #region Properties
        int Width { get; }
        int Height { get; }
        #endregion

        #region Methods
        void NewGame();
        ICardModel GetContents(Coord coord);
        ICardModel At(Coord coord);
        bool IsValidCoord(Coord coord);
        IEnumerable<ICardModel> GetContents();
        void PlaceCard(ICardModel cardModel, Coord coord);
        bool CanPlaceCard(ICardModel card, Coord coord);
        IEnumerable<PlayCard> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<ICardModel> AttackedCards(Coord cood);
        IEnumerable<ICardModel> DefendededCards(ICardModel defender, Coord cood);
        IEnumerable<ICardModel> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);

        string Print();
		string Print(Func<Coord, string> fun);
        string CardToRep(ICardModel cardModel);
        #endregion
    }
}
