using System.Collections.Generic;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Mock.Agent
{
    using App.Agent;

    public class WhitePlayerAgent
        : PlayerAgentBase
        , IWhitePlayerAgent
    {
        public WhitePlayerAgent(IPlayerModel model) : base(model)
        {
        }

        public override ITransient StartGame()
        {
            throw new System.NotImplementedException();
        }

        public override IFuture<List<ICardModel>> Mulligan()
        {
            throw new System.NotImplementedException();
        }

        public override IFuture<MovePiece> PlaceKing()
        {
            throw new System.NotImplementedException();
        }

        public override ITransient TurnStart()
        {
            throw new System.NotImplementedException();
        }

        public override IFuture<IRequest> NextRequest()
        {
            throw new System.NotImplementedException();
        }

        public override ITransient TurnEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}
