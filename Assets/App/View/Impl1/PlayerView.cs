using System;
using System.Collections.Generic;
using App.Agent;
using App.Common;
using App.Common.Message;
using App.Registry;

namespace App.View.Impl1
{
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
            Info($"{request}");
            _requests.Add(new Turnaround(request, response));

        }

        class Turnaround
        {
            public IRequest Request;
            public Action<IResponse> Responder;

            public Turnaround(IRequest request, Action<IResponse> responder)
            {
                Request = request;
                Responder = responder;
            }
        }

        private readonly List<Turnaround> _requests = new List<Turnaround>();
    }
}
