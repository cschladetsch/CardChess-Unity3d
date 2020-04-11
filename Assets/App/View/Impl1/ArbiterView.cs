namespace App.View.Impl1
{
<<<<<<< HEAD
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
=======
    using Dekuple;
    using Dekuple.View.Impl;
    using UniRx;
    using UnityEngine;
    using Agent;
    using Common;
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9

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
<<<<<<< HEAD
=======
        public EndTurnButtonView WhiteEndButton;
        public EndTurnButtonView BlackEndButton;
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
        public EColor CurrentPlayerColor => Agent.CurrentPlayerAgent.Value.Model.Color;
        public AudioClip[] MusicClips;
        public AudioClip[] EndTurnClips;
        
        public IPlayerView WhitePlayerView => WhitePlayer;
        public IPlayerView BlackPlayerView => BlackPlayer;
        public IPlayerView CurrentPlayerView => CurrentPlayerColor == EColor.White ? WhitePlayerView : BlackPlayerView;
        public IBoardView BoardView => Board;

        private GameRoot _gameRoot;

<<<<<<< HEAD
        //public override void SetAgent(IPlayerView view, IArbiterAgent agent)
        public override void SetAgent(IAgent agent)
        {
            base.SetAgent(agent);
=======
        public void SetAgent(IPlayerView view, IArbiterAgent agent)
        //public override void SetAgent(IViewBase view, IAgent agent)
        {
            _gameRoot = transform.parent.GetComponent<GameRoot>();

            base.SetAgent(view, agent);
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9

            PlayMusic();

            WhitePlayerView.SetAgent(Agent.WhitePlayerAgent);
            BlackPlayerView.SetAgent(Agent.BlackPlayerAgent);

<<<<<<< HEAD
            var arbiterModel = Model as IArbiterModel;
            arbiterModel.GameState.DistinctUntilChanged().Subscribe(c => StateText.text = $"{c}").AddTo(this);
            arbiterModel.CurrentPlayer.DistinctUntilChanged().Subscribe(c => CurrentPlayerText.text = $"{c}").AddTo(this);
=======
            var model = Agent.Model;
            model.GameState.DistinctUntilChanged().Subscribe(c => StateText.text = $"{c}").AddTo(this);
            model.CurrentPlayer.DistinctUntilChanged().Subscribe(c => CurrentPlayerText.text = $"{c.Color}").AddTo(this);
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9

            Agent.LastResponse.Subscribe( (r) =>
            {
                #if DEBUG
                _gameRoot.CheckAllValid();
                #endif
            }).AddTo(this);
        }

<<<<<<< HEAD
            Agent.LastResponse.Subscribe((r) => _gameRoot.CheckAllValid());
=======
        public bool CurrentPlayerOwns(IOwned owned)
        {
            Assert.IsTrue(IsValid);
            Assert.IsNotNull(owned);
            Assert.IsNotNull(owned.Owner);
            return Agent.CurrentPlayerAgent.Value.Model == owned.Owner.Value;
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
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


