using System;
using Dekuple;
using Dekuple.View;
using Dekuple.View.Impl;

namespace App.View.Impl1
{
    using Agent;

    public class PlayerView
        : ViewBase<IPlayerAgent>
        , IPlayerView
    {
        // I really wish these could be interfaces.
        // They should probably end up being Prefabs that can be
        // searched for Components that implement the correct interfaces.
        public ManaView ManaView;
        public HandView Hand;
        public DeckView Deck;
        public EndTurnButtonView EndTurnButton;

        //[Inject] public IArbiterView ArbiterView;

        public override void SetAgent(IPlayerView view, IPlayerAgent agent)
        {
            Assert.IsNotNull(agent);
            Assert.IsNotNull(agent.Hand);
            Assert.IsNotNull(agent.Deck);

            base.SetAgent(view, agent);
            Deck.SetAgent(this, Agent.Deck);
            Hand.SetAgent(this, Agent.Hand);
            EndTurnButton.SetAgent(this, Agent.EndTurnButton);
            ManaView.SetAgent(this, Agent);
        }

        public void PushRequest(IRequest request, Action<IResponse> response)
        {
            Assert.IsNotNull(request);
            Assert.IsNotNull(response);

            Agent.PushRequest(request, response);
        }
    }
}
