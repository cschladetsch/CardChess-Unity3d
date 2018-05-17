using System;
using System.Collections;
using System.Collections.Generic;
using App.Action;
using Flow;

namespace App.Agent
{
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
            throw new System.NotImplementedException();
        }

        public IEnumerable<ICardInstance> AdjacentTo(Coord coord, int dist = 1)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Coord> GetMovements(Coord coord)
        {
            return Model.GetMovements(coord);
        }

		public string ToString(Func<Coord, string> func)
		{
			return Model.ToString(func);
		}

		public override string ToString()
		{
			return Model.ToString();
		}
	}
}
