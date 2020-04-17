namespace App.View.Impl
{
    using Dekuple;
    using Dekuple.Agent;
    using Dekuple.View.Impl;
    using Agent;

    /// <summary>
    /// Common for many game object views.
    /// </summary>
    public class GameViewBase
        : ViewBase
        , IGameViewBase
    {
        // TODO this is a nightmare
        public IPlayerAgent PlayerAgent => ArbiterView.GetPlayerView(AgentBase).AgentBase as IPlayerAgent;
        
        [Inject] public IArbiterView ArbiterView { get; set; }
        [Inject] public IBoardView BoardView { get; set; }

        protected bool IsCurrentPlayer() => ArbiterView.CurrentPlayerOwns(this);
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
