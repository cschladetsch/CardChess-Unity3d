using System;
using App.Agent;
using App.Common;
using App.Common.Message;
using App.Model;
using UniRx;

namespace App.View.Impl1
{
    public class ArbiterView
        : ViewBase<IArbiterAgent>
        , IArbiterView
    {
        public BoardView Board;
        public PlayerView WhitePlayerView;
        public PlayerView BlackPlayerView;
        public TMPro.TextMeshPro CurrentPlayerText;
        public TMPro.TextMeshPro ResponseText;
        public TMPro.TextMeshPro StateText;

        public override void SetAgent(IPlayerView view, IArbiterAgent agent)
        {
            base.SetAgent(view, agent);

            WhitePlayerView.SetAgent(WhitePlayerView, Agent.WhitePlayerAgent);
            BlackPlayerView.SetAgent(BlackPlayerView, Agent.BlackPlayerAgent);

            var model = Agent.Model;
            model.GameState.DistinctUntilChanged().Subscribe(c => StateText.text = $"{c}").AddTo(this);
            model.CurrentPlayer.DistinctUntilChanged().Subscribe(c => CurrentPlayerText.text = $"{c}").AddTo(this);
        }

        public override void Create()
        {
        }

        protected override void Step()
        {
            base.Step();
            Agent.Step();
        }
    }
}
