using System;
using System.Collections.Generic;
using App.Agent;

namespace App.Model
{
    using Action;
    using Common;

    /// <summary>
    /// The NxN playing boardAgent. Typically 6x6, 10x10, or 12x12 (Desktop only)
    /// </summary>
    public interface IBoardModel :
        IModel,
        ICreateWith<int, int>
    {
        #region Properties
        int Width { get; }
        int Height { get; }
        #endregion

        #region Methods
        void NewGame();
        Agent.ICardAgent GetContents(Coord coord);
        Agent.ICardAgent At(Coord coord);
        bool IsValidCoord(Coord coord);
        IEnumerable<Agent.ICardAgent> GetContents();
        void PlaceCard(Agent.ICardAgent cardAgent, Coord coord);
        bool CanPlaceCard(Agent.ICardAgent cardAgent, Coord coord);
        IEnumerable<PlayCard> GetAdjacent(Coord cood, int dist = 1);
        IEnumerable<Agent.ICardAgent> AttackedCards(Coord cood);
        IEnumerable<Agent.ICardAgent> DefendededCards(Agent.ICardAgent defender, Coord cood);
        IEnumerable<Agent.ICardAgent> Defenders(Coord cood);
        IEnumerable<Coord> GetMovements(Coord cood);

		string ToString(Func<Coord, string> fun);
        string CardToRep(Agent.ICardAgent cardAgent);

        #endregion
    }
}
