namespace App.View.Impl1
{
    using Dekuple;
    using Dekuple.View.Impl;
    using UniRx;
    using UnityEngine;
    using Agent;
    using Common;

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
        public TMPro.TextMeshPro ResponseText;
        public TMPro.TextMeshPro StateText;
        public EndTurnButtonView WhiteEndButton;
        public EndTurnButtonView BlackEndButton;
        public EColor CurrentPlayerColor => Agent.CurrentPlayerAgent.Value.Model.Color;
        public AudioClip[] MusicClips;
        public AudioClip[] EndTurnClips;
        
        public IPlayerView WhitePlayerView => WhitePlayer;
        public IPlayerView BlackPlayerView => BlackPlayer;
        public IPlayerView CurrentPlayerView => CurrentPlayerColor == EColor.White ? WhitePlayerView : BlackPlayerView;
        public IBoardView BoardView => Board;

        private GameRoot _gameRoot;

        public void SetAgent(IPlayerView view, IArbiterAgent agent)
        //public override void SetAgent(IViewBase view, IAgent agent)
        {
            _gameRoot = transform.parent.GetComponent<GameRoot>();

            base.SetAgent(view, agent);

            PlayMusic();

            WhitePlayerView.SetAgent(WhitePlayerView, Agent.WhitePlayerAgent);
            BlackPlayerView.SetAgent(BlackPlayerView, Agent.BlackPlayerAgent);

            var model = Agent.Model;
            model.GameState.DistinctUntilChanged().Subscribe(c => StateText.text = $"{c}").AddTo(this);
            model.CurrentPlayer.DistinctUntilChanged().Subscribe(c => CurrentPlayerText.text = $"{c.Color}").AddTo(this);

            Agent.LastResponse.Subscribe( (r) =>
            {
                #if DEBUG
                _gameRoot.CheckAllValid();
                #endif
            }).AddTo(this);
        }

        public bool CurrentPlayerOwns(IOwned owned)
        {
            Assert.IsTrue(IsValid);
            Assert.IsNotNull(owned);
            Assert.IsNotNull(owned.Owner);
            return Agent.CurrentPlayerAgent.Value.Model == owned.Owner.Value;
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
    }
}
