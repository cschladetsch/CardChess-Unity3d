using System;
using System.Collections.Generic;
using App.Action;
using Flow;

namespace App.Agent
{
    public interface IBoard : IAgent<Model.IBoard>
    {
        void NewGame();
        IGenerator PlaceCard(ICardInstance card, Coord where);
        ICardInstance At(Coord coord);
        IEnumerable<PlayCard> AdjacentTo(Coord coord, int dist = 1);
        IEnumerable<Coord> GetMovements(Coord coord);
		string ToString(Func<Coord, string> fun);
        string CardToRep(ICardInstance card);
    }
}
