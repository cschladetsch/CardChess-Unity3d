using App.Agent;
using App.Common;

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

        public override bool Construct(IPlayerAgent agent)
        {
            Assert.IsNotNull(agent);
            Assert.IsNotNull(agent.Hand);
            Assert.IsNotNull(agent.Deck);

            if (!base.Construct(agent))
                return false;

            Deck.Construct(Agent.Deck);
            Hand.Construct(Agent.Hand);
            ManaView.Construct(Agent);

            return true;
        }
    }
}
