using System;
using System.Collections.Generic;

namespace App.Model
{
    using Action;
    using Common;
    using ICard = Agent.ICardInstance;

    /// <summary>
    /// The NxN playing board. Typically 6x6, 10x10, or 12x12 (Desktop only)
    /// </summary>
    public interface IBoard :
        IModel,
        ICreateWith<int, int>
    {
        #region Properties
        int Width { get; }
        int Height { get; }
        #endregion

        #region Methods
        void NewGame();
        ICard GetContents(Coord coord);
        ICard At(Coord coord);
        bool IsValidCoord(Coord coord);
        IEnumerable<ICard> GetContents();
        void PlaceCard(ICard card, Coord coord);
        bool CanPlaceCard(ICard card, Coord coord);
        IEnumerable<PlayCard> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<ICard> AttackedCards(Coord cood);
        IEnumerable<ICard> DefendededCards(ICard defender, Coord cood);
        IEnumerable<ICard> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);

		string ToString(Func<Coord, string> fun);
        #endregion
    }
}
