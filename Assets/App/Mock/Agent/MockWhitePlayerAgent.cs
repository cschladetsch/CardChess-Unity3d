using System;
using App.Common;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Mock.Agent
{
    using App.Agent;

    public class MockWhitePlayerAgent
        : PlayerAgentBase
        , IWhitePlayerAgent
    {
        public MockWhitePlayerAgent(IPlayerModel model)
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
            return New.Future(new RejectCards(Model));
        }

        public override IFuture<PlacePiece> PlaceKing()
        {
            Info($"{this} places king");
            return New.Future(new PlacePiece(Model, Model.King, new Coord(4, 2)));
        }

        public override ITransient TurnStart()
        {
            return New.Nop();
        }

        public override ITransient TurnEnd()
        {
            Info("Turn End");
            return New.Nop();
        }
    }
}
