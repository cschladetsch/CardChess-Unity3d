using App.Model;
using UnityEngine.Audio;

namespace App.View.Impl
{
    using UnityEngine;
    using UniRx;
    using Dekuple;
    using Dekuple.Agent;
    using Dekuple.View.Impl;
    using Agent;

    /// <summary>
    /// View of an Arbiter.
    /// </summary>
    public class ArbiterView
        : ViewBase<IArbiterAgent>
        , IArbiterView
    {
        public BoardView Board;
        public TMPro.TextMeshPro CurrentPlayerText;
        public TMPro.TextMeshPro StateText;
        public AudioClip[] MusicClips;
        public AudioClip[] EndTurnClips;
        public PlayerView WhitePlayer;
        public PlayerView BlackPlayer;
        
        public IPlayerView WhitePlayerView => WhitePlayer;
        public IPlayerView BlackPlayerView => BlackPlayer;
        public IBoardView BoardView => Board;

        public AudioMixer AuduiMusic;
        public AudioMixer AudioSfx;
        public AudioMixer AudioBattle;

        public override void SetAgent(IAgent agent)
        {
            var arbiterAgent = agent as IArbiterAgent;
            Assert.IsNotNull(arbiterAgent);
            base.SetAgent(arbiterAgent);
            
            // PlayMusic();

            WhitePlayerView.SetAgent(Agent.WhitePlayerAgent);
            BlackPlayerView.SetAgent(Agent.BlackPlayerAgent);

            var model = Agent.Model;
            model.GameState.DistinctUntilChanged().Subscribe(
                c => StateText.text = $"{GetPlayText(c)}").AddTo(this);
            model.CurrentPlayer.DistinctUntilChanged().Subscribe(
                c => CurrentPlayerText.text = $"{c.Color}").AddTo(this);
        }

        private string GetPlayText(EGameState state)
        {
            switch (state)
            {
                case EGameState.PlayTurn: return "Play";
            }

            return state.ToString();
        }

        public bool CurrentPlayerOwns(IOwned owned)
        {
            Assert.IsTrue(IsValid);
            Assert.IsNotNull(owned);
            Assert.IsNotNull(owned.Owner);
            return Agent.CurrentPlayerAgent.Value.Model == owned.Owner.Value;
        }

        public IPlayerView GetPlayerView(IAgent agent)
            => WhitePlayerView.SameOwner(agent) ? WhitePlayerView : BlackPlayerView;

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


