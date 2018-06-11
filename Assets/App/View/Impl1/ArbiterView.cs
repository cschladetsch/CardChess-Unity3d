using System.Linq;
using App.Common;
using App.Mock.Model;
using UniRx;

namespace App.View.Impl1
{
    using Agent;
    using Model;

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

            CheckValidHands();
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

        private void CheckValidHands()
        {
            var reg = WhitePlayerView.PlayerModel.Registry;
            var numHandModels = 0;
            foreach (var m in reg.Instances)
            {
                var hand = m as IHandModel;
                if (hand == null)
                    continue;
                numHandModels++;
            }

            var numHandAgents = 0;
            foreach (var m in WhitePlayerView.Agent.Registry.Instances)
            {
                var hand = m as IHandAgent;
                if (hand == null)
                    continue;
                numHandAgents++;
            }

            var numHandViews = 0;
            foreach (var m in WhitePlayerView.Registry.Instances)
            {
                var hand = m as IHandView;
                if (hand == null)
                    continue;
                //Warn($"Found hand {hand}");
                numHandViews++;
            }
            Assert.AreEqual(2, numHandModels);
            Assert.AreEqual(2, numHandAgents);
            Assert.AreEqual(2, numHandViews);
        }
    }
}
