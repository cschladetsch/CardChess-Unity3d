using System;
using Dekuple;
using Flow;

namespace App.Mock.Agent
{
    using App.Agent;
    using App.Model;
    using Common.Message;

    public class MockWhitePlayerAgent
        : PlayerAgentBase
        , IWhitePlayerAgent
    {
        public MockWhitePlayerAgent(IPlayerModel model)
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
            return New.Future(new RejectCards(Model));
        }

        public override ITransient TurnStart()
        {
            return New.Nop();
        }

        public override ITimedFuture<Turnaround> NextRequest(float seconds)
        {
            var req = Model.NextAction();
            return New.TimedFuture(TimeSpan.FromSeconds(seconds), new Turnaround(req, ResponseHandler));
        }

        public override ITransient TurnEnd()
        {
            Info("Turn End");
            return New.Nop();
        }
    }
}
