using System;

using Dekuple;
using Dekuple.Agent;
using Dekuple.Model;
using Dekuple.View;
using Dekuple.View.Impl;

namespace App.View.Impl1
{
    using Agent;

    /// <summary>
    /// View of a player's representative in the scene.
    /// </summary>
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

        public override void SetAgent(IAgent agent)
        {
            Assert.IsNotNull(agent);
            var player = agent as IPlayerAgent;
            Assert.IsNotNull(player);
            Assert.IsNotNull(player.Hand);
            Assert.IsNotNull(player.Deck);

            base.SetAgent(agent);
            Hand.SetAgent(player.Hand);
            ManaView.SetAgent(Agent);
            Deck.SetAgent(player.Deck);
            EndTurnButton.SetAgent(player.EndTurnButton);
        }

        public void PushRequest(IRequest request, Action<IResponse> response)
        {
            Assert.IsNotNull(request);
            Assert.IsNotNull(response);

            Agent.PushRequest(request, response);
        }
    }
}
