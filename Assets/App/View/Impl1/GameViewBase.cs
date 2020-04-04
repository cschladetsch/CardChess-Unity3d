using App.Model;
using Dekuple;
using Dekuple.View.Impl;

namespace App.View.Impl1
{
    public class GameViewBase
        : ViewBase
        , IGameViewBase
    {
        public IPlayerView PlayerView { get; set; }
        public IPlayerModel PlayerModel => Owner.Value as IPlayerModel;

        [Inject] public IArbiterView ArbiterView { get; set; }
        [Inject] public IBoardView BoardView { get; set; }

        protected bool IsCurrentPlayer() => ArbiterView.CurrentPlayerOwns(this);
    }

    public class GameViewBase<TAgent>
        : GameViewBase
    {
        public TAgent Agent { get; set; }
    }
}
