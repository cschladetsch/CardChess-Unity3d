using System;
using System.Collections.Generic;
using App.Action;
using Flow;

namespace App.Agent
{
    public interface IBoardAgent : IAgent<Model.IBoardModel>
    {
        void NewGame();
        IGenerator PlaceCard(ICardAgent cardAgent, Coord where);
        ICardAgent At(Coord coord);
        IEnumerable<PlayCard> AdjacentTo(Coord coord, int dist = 1);
        IEnumerable<Coord> GetMovements(Coord coord);
		string ToString(Func<Coord, string> fun);
        string CardToRep(ICardAgent cardAgent);
    }
}
