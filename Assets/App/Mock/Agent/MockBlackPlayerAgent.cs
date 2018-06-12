using System;
using System.Collections.Generic;
using App.Common;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Mock.Agent
{
    using App.Agent;

    public class MockBlackPlayerAgent
        : PlayerAgentBase
        , IBlackPlayerAgent
    {
        public MockBlackPlayerAgent(IPlayerModel model)
            : base(model)
        {
        }

        public override void StartGame()
        {
            // TODO: Some sort of animation
            base.StartGame();
        }

        public override IFuture<RejectCards> Mulligan()
        {
            // keep all cards
            return New.Future(new RejectCards(Model));
        }

        public override ITransient TurnStart()
        {
            return null;//New.Nop();
        }

        public override ITimedFuture<Turnaround> NextRequest(float seconds)
        {
            var req = Model.NextAction();
            return New.TimedFuture(TimeSpan.FromSeconds(seconds), new Turnaround(req, ResponseHandler));
        }

        public override ITransient TurnEnd()
        {
            Info("Turn End");
            return null;
        }
    }
}
