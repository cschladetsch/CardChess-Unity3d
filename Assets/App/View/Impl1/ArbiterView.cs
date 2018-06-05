using App.Agent;
using UnityEditor;

namespace App.View.Impl1
{
    public class ArbiterView
        : ViewBase<IArbiterAgent>
    {
        public BoardView Board;
        public PlayerView White;
        public PlayerView Black;
        public TMPro.TextMeshPro CurrentPlayerText;

        public override bool Construct(IArbiterAgent agent)
        {
            if (!base.Construct(agent))
                return false;

            White.Construct(Agent.WhitePlayer);
            Black.Construct(Agent.BlackPlayer);
            return true;
        }

        protected override void Begin()
        {
            base.Begin();
        }
    }
}
