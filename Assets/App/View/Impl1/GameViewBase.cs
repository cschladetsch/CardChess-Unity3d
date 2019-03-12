using App.Model;
using Dekuple;
using Dekuple.Agent;
using Dekuple.View.Impl;

namespace App.View.Impl1
{
    public class GameViewBase
        : ViewBase
        , IGameViewBase
    {
        public IPlayerView PlayerView => OwnerView as IPlayerView;
        public IPlayerModel PlayerModel => Owner.Value as IPlayerModel;

        [Inject] public IArbiterView ArbiterView { get; set; }
        [Inject] public IBoardView BoardView { get; set; }

        protected bool IsCurrentPlayer()
        {
            return ArbiterView.CurrentPlayerOwns(this);
        }
    }

    public class GameViewBase<TAgent>
        : GameViewBase
        where TAgent
            : class
            , IAgent
    {
        public TAgent Agent => AgentBase as TAgent;
    }
}
