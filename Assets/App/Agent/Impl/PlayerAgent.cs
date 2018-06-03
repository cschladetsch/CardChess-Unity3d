using System;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Agent
{
    class PlayerAgent
        : PlayerAgentBase
    {
        public PlayerAgent(IPlayerModel model)
            : base(model)
        {
        }

        public override IFuture<RejectCards> Mulligan()
        {
            return null;
        }

        public override IFuture<PlacePiece> PlaceKing()
        {
            return null;
        }

        public override ITransient TurnStart()
        {
            return null;
        }

        public override ITimedFuture<IRequest> NextRequest(float seconds)
        {
            return null;
        }

        public override ITransient TurnEnd()
        {
            return null;
        }
    }
}
