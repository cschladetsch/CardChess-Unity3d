using App.Common.Message;
using UnityEngine.UI;
using UniRx;

namespace App.View.Impl1
{
    using Agent;
    using Common;

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
        public Button WhiteEnd;
        public Button BlackEnd;

        public override void SetAgent(IPlayerView view, IArbiterAgent agent)
        {
            base.SetAgent(view, agent);

            WhitePlayerView.SetAgent(WhitePlayerView, Agent.WhitePlayerAgent);
            BlackPlayerView.SetAgent(BlackPlayerView, Agent.BlackPlayerAgent);

            var model = Agent.Model;
            model.GameState.DistinctUntilChanged().Subscribe(c => StateText.text = $"{c}").AddTo(this);
            model.CurrentPlayer.DistinctUntilChanged().Subscribe(c => CurrentPlayerText.text = $"{c}").AddTo(this);

            SetupUi();
        }

        public void SetupUi()
        {
            Agent.PlayerAgent.Subscribe(player =>
            {
                WhiteEnd.interactable = player.Color == EColor.White;
                BlackEnd.interactable = player.Color == EColor.Black;
            });

            var whiteAgent = WhitePlayerView.Agent;
            var blackAgent = BlackPlayerView.Agent;
            var white = whiteAgent.Model;
            var black = blackAgent.Model;
            WhiteEnd.Bind(() => whiteAgent.PushRequest(new TurnEnd(white), TurnEnded));
            WhiteEnd.Bind(() => blackAgent.PushRequest(new TurnEnd(black), TurnEnded));
        }

        private void TurnEnded(IResponse obj)
        {
            Info($"TurnEnded for {obj.Request.Player}");
        }

        public void AddWhiteCard()
        {
            var model = WhitePlayerView.Agent.Model;
            var card = model.RandomCard();
            model.Hand.Add(card);
        }

        protected override void Step()
        {
            base.Step();
            Agent?.Step();
        }
    }
}
