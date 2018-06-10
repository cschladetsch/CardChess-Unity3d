using App.Mock.Model;
using UniRx;

namespace App.View.Impl1
{
    using Agent;

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

        public void AddWhiteCard()
        {
            var model = WhitePlayerView.Agent.Model;
            var card = model.RandomCard();
            model.Hand.Add(card);
        }

        public override void Create()
        {
        }

        protected override void Step()
        {
            base.Step();
            Agent?.Step();
        }
    }
}
