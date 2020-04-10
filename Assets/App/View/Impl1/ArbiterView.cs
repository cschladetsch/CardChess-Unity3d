namespace App.View.Impl1
{
    using UnityEngine;
    using UnityEngine.UI;
    using UniRx;
    using Dekuple;
    using Dekuple.Agent;
    using Dekuple.Model;
    using Dekuple.View.Impl;
    using Common;
    using Common.Message;
    using Agent;
    using Model;

    /// <summary>
    /// View of an Arbiter. This is mostly related to meta-data about the game state.
    /// </summary>
    public class ArbiterView
        : ViewBase<IArbiterAgent>
        , IArbiterView
    {
        public BoardView Board;
        public PlayerView WhitePlayer;
        public PlayerView BlackPlayer;
        public TMPro.TextMeshPro CurrentPlayerText;
        public TMPro.TextMeshPro StateText;
        public EColor CurrentPlayerColor => Agent.CurrentPlayerAgent.Value.Model.Color;
        public AudioClip[] MusicClips;
        public AudioClip[] EndTurnClips;
        public IPlayerView WhitePlayerView => WhitePlayer;
        public IPlayerView BlackPlayerView => BlackPlayer;
        public IPlayerView CurrentPlayerView => CurrentPlayerColor == EColor.White ? WhitePlayerView : BlackPlayerView;
        public IBoardView BoardView => Board;

        private GameRoot _gameRoot;

        //public override void SetAgent(IPlayerView view, IArbiterAgent agent)
        public override void SetAgent(IAgent agent, IModel model)
        {
            base.SetAgent(agent, model);

            PlayMusic();

            WhitePlayerView.SetAgent(Agent.WhitePlayerAgent, Agent.WhitePlayerAgent.Model);
            BlackPlayerView.SetAgent(Agent.BlackPlayerAgent, Agent.BlackPlayerAgent.Model);

            var arbiterModel = model as IArbiterModel;
            arbiterModel.GameState.DistinctUntilChanged().Subscribe(c => StateText.text = $"{c}").AddTo(this);
            arbiterModel.CurrentPlayer.DistinctUntilChanged().Subscribe(c => CurrentPlayerText.text = $"{c}").AddTo(this);

            _gameRoot = transform.parent.GetComponent<GameRoot>();

            Agent.LastResponse.Subscribe((r) => _gameRoot.CheckAllValid());
        }

        private void PlayMusic()
        {
            _AudioSource.clip = MusicClips[0];
            _AudioSource.loop = true;
            _AudioSource.volume = 0.5f;
            _AudioSource.Play();
        }

        protected override void Step()
        {
            base.Step();
            Agent?.Step();
        }

        public bool CurrentPlayerOwns(IOwned owned)
        {
            Assert.IsTrue(IsValid);
            Assert.IsNotNull(owned);
            Assert.IsNotNull(owned.Owner);
            return Agent.CurrentPlayerAgent.Value.Model == owned.Owner.Value;
        }
    }
}


