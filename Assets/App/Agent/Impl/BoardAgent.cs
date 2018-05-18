using System;
using System.Collections;
using System.Collections.Generic;
using App.Action;
using Flow;

namespace App.Agent
{
    using Common;

    public class BoardAgent :
        AgentBaseCoro<Model.IBoardModel>,
        IBoardAgent
    {
        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }

        public void NewGame()
        {
            Model.NewGame();
        }

        public IGenerator PlaceCard(ICardAgent cardAgent, Coord coord)
        {
            if (!Model.CanPlaceCard(cardAgent.Model, coord))
                return null;
            return New.Do(() => Model.PlaceCard(cardAgent.Model, coord));
        }

        public ICardAgent At(Coord coord)
        {
            //TODOreturn Model.At(coord);
            return null;
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

        public string CardToRep(ICardAgent cardAgent)
        {
            return Model.CardToRep(cardAgent.Model);
        }

        public override string ToString()
        {
            return Model.ToString();
        }
    }
}
