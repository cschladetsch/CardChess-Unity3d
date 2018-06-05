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
        public GameRoot Root;

        protected override void Begin()
        {
            base.Begin();

            White.Construct(Root.WhiteAgent);
            Black.Construct(Root.BlackAgent);
        }
    }
}
