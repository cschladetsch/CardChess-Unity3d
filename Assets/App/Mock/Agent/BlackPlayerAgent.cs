using System.Collections.Generic;
using App.Common;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Mock.Agent
{
    using App.Agent;

    public class BlackPlayerAgent
        : PlayerAgentBase
        , IBlackPlayerAgent
    {
        public BlackPlayerAgent(IPlayerModel model)
            : base(model)
        {
        }

        public override ITransient StartGame()
        {
            // TODO: Some sort of animation
            return base.StartGame();
        }

        public override IFuture<RejectCards> Mulligan()
        {
            // keep all cards
            return New.Future(new RejectCards(Model));
        }

        public override IFuture<PlacePiece> PlaceKing()
        {
            Info($"{this} places king");
            return New.Future(new PlacePiece(Model, Model.King, new Coord(4, 5)));
        }

        public override ITransient TurnStart()
        {
            return null;//New.Nop();
        }

        public override IFuture<IRequest> NextRequest()
        {
            Info($"{this} passes");
            return null;
        }

        public override ITransient TurnEnd()
        {
            Info("Turn End");
            return null;
        }
    }
}
