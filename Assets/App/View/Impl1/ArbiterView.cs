using App.Agent;
using App.Common;

namespace App.View.Impl1
{
    public class ArbiterView
        : ViewBase<IArbiterAgent>
    {
        public BoardView Board;
        public PlayerView White;
        public PlayerView Black;
        public TMPro.TextMeshPro CurrentPlayerText;

        public override void SetAgent(IArbiterAgent agent)
        {
            base.SetAgent(agent);

            Assert.IsNotNull(agent);
            Assert.IsNotNull(agent.WhitePlayer);
            Assert.IsNotNull(agent.BlackPlayer);

            White.SetAgent(Agent.WhitePlayer);
            Black.SetAgent(Agent.BlackPlayer);
        }

        protected override void Begin()
        {
            base.Begin();
        }
    }
}
