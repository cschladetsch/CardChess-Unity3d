using System;

namespace Dekuple.View.Impl1
{
    using Agent;
    using Common;
    using Common.Message;
    using Registry;

    public class PlayerView
        : ViewBase<IPlayerAgent>
        , IPlayerView
    {

        public override void SetAgent(IPlayerView view, IPlayerAgent agent)
        {
            Assert.IsNotNull(agent);
            //Assert.IsNotNull(agent.Hand);
            //Assert.IsNotNull(agent.Deck);

            base.SetAgent(view, agent);
            //Deck.SetAgent(this, Agent.Deck);
            //Hand.SetAgent(this, Agent.Hand);
            //EndTurnButton.SetAgent(this, Agent.EndTurnButton);
            //ManaView.SetAgent(this, Agent);
        }

        public void PushRequest(IRequest request, Action<IResponse> response)
        {
            Assert.IsNotNull(request);
            Assert.IsNotNull(response);

            Agent.PushRequest(request, response);
        }
    }
}
