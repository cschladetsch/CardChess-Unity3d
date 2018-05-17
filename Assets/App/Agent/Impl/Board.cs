using System;
using System.Collections;
using System.Collections.Generic;
using App.Action;
using Flow;

namespace App.Agent
{
    using Common;

    public class Board : AgentBaseCoro<Model.IBoard>, IBoard
    {
        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }

        public void NewGame()
        {
            Model.NewGame();
        }

        public IGenerator PlaceCard(ICardInstance card, Coord coord)
        {
            if (!Model.CanPlaceCard(card, coord))
                return null;
            Model.PlaceCard(card, coord);
            return null;
        }

        public ICardInstance At(Coord coord)
        {
            return Model.At(coord);
        }

        public IEnumerable<PlayCard> AdjacentTo(Coord coord, int dist = 1)
        {
            return Model.GetAdjacent(coord, dist);
        }

        public IEnumerable<Coord> GetMovements(Coord coord)
        {
            return Model.GetMovements(coord);
        }

        public string ToString(Func<Coord, string> func)
        {
            return Model.ToString(func);
        }

        public string CardToRep(ICardInstance card)
        {
            return Model.CardToRep(card);
        }

        public override string ToString()
        {
            return Model.ToString();
        }
    }
}
