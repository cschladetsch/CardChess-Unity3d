using System;
using System.Collections;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Agent
{
    using Common;

    public class BoardAgent
        : AgentBaseCoro<Model.IBoardModel>
        , IBoardAgent
    {
        public BoardAgent(IBoardModel model)
            : base(model)
        {
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }

        public ITransient NewGame()
        {
            Model.NewGame();
            return null;
        }

        public IPieceAgent At(Coord coord)
        {
            return null;
        }

        public Response Set(IPieceAgent piece, Coord cood)
        {
            throw new NotImplementedException();
        }
    }
}
