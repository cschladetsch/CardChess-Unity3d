using System;

namespace App.View.Impl1
{
    using Agent;
    using Common;
    using Common.Message;
    using Registry;

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

        [Inject] public IArbiterView ArbiterView;

        public override void SetAgent(IPlayerView view, IPlayerAgent agent)
        {
            Assert.IsNotNull(agent);
            Assert.IsNotNull(agent.Hand);
            Assert.IsNotNull(agent.Deck);

            base.SetAgent(view, agent);

            Deck.SetAgent(this, Agent.Deck);
            Hand.SetAgent(this, Agent.Hand);
            ManaView.SetAgent(this, Agent);
        }

        public void NewRequest(IRequest request, Action<IResponse> response)
        {
            Agent.PushRequest(new Turnaround(request, response));
        }
    }
}
