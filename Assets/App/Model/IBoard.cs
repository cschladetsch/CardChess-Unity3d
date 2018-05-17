using System;
using System.Collections.Generic;
using App.Agent;

namespace App.Model
{
    using Action;
    using Common;

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
        Agent.ICard GetContents(Coord coord);
        Agent.ICard At(Coord coord);
        bool IsValidCoord(Coord coord);
        IEnumerable<Agent.ICard> GetContents();
        void PlaceCard(Agent.ICard card, Coord coord);
        bool CanPlaceCard(Agent.ICard card, Coord coord);
        IEnumerable<PlayCard> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<Agent.ICard> AttackedCards(Coord cood);
        IEnumerable<Agent.ICard> DefendededCards(Agent.ICard defender, Coord cood);
        IEnumerable<Agent.ICard> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);

		string ToString(Func<Coord, string> fun);
        string CardToRep(Agent.ICard card);

        #endregion
    }
}
