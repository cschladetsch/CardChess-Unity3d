using System.Linq;

namespace App
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Flow;
    using UniRx;
    using Dekuple;
    using Dekuple.Agent;
    using Common.Message;
    using Agent;
    using Model;
    using Common;

    /// <inheritdoc cref="AgentBaseCoro{TModel}" />
    /// <summary>
    /// The 'Adjudicator' of the game: controls the sequencing of the events
    /// but not all the rules.
    ///
    /// Responsible for enforcing the rules of the game are shared with
    /// the Board- and Card-Agents and Models.
    /// </summary>
    public class ArbiterAgent
        : AgentBaseCoro<IArbiterModel>
        , IArbiterAgent
    {
        [Inject] public IBoardAgent BoardAgent { get; set; }

        public IReadOnlyReactiveProperty<RequestResponse> LastResponse => Model.LastResponse;
        public IReadOnlyReactiveProperty<EGameState> GameState => Model.GameState;
        public IReadOnlyReactiveProperty<IPlayerAgent> CurrentPlayerAgent => _playerAgent;
        public IReadOnlyReactiveProperty<string> Log => Model.Log;

        public IPlayerAgent WhitePlayerAgent => _playerAgents[0];
        public IPlayerAgent BlackPlayerAgent => _playerAgents[1];
        public IPlayerModel CurrentPlayerModel => CurrentPlayerAgent.Value.Model;

        private float _timeOut;
        private DateTime _timeStart;
        private List<IPlayerAgent> _playerAgents = new List<IPlayerAgent>();
        private readonly ReactiveProperty<IPlayerAgent> _playerAgent = new ReactiveProperty<IPlayerAgent>();

        public ArbiterAgent(IArbiterModel model)
            : base(model)
        {
        }

        public void Step()
        {
            Kernel.Step();
        }

        public ITransient PrepareGame(IPlayerAgent white, IPlayerAgent black)
        {
            Assert.IsNotNull(white);
            Assert.IsNotNull(black);

            Model.PrepareGame(white.Model, black.Model);
            _playerAgents = new List<IPlayerAgent> {white, black};
            Model.CurrentPlayer.Subscribe(SetPlayerAgent).AddTo(this);

            // TODO: do some animations etc
            return null;
        }

        private void SetPlayerAgent(IPlayerModel model)
        {
            var agent = _playerAgents.FirstOrDefault(p => p.Model == model);
            if (agent == null)
                throw new Exception($"Player agent for {model} not found");
                
            _playerAgent.Value = agent;
        }

        public void StartGame()
        {
            Info($"{this} StartGame");
            BoardAgent.StartGame();
            _Node.Add(New.Coroutine(PlayerTurn).Named("ArbiterMain"));
        }

        private IEnumerator PlayerTurn(IGenerator self)
        {
            CurrentPlayerModel.StartTurn();

            ResetTurnTimer();

            // player can make as many valid actions as he can during his turn
            while (true)
            {
                if (GameState.Value == EGameState.Completed)
                {
                    self.Complete();
                    yield break;
                }

                Assert.IsTrue(self.Active);

                var future = CurrentPlayerAgent.Value.NextRequest(_timeOut);
                Assert.IsNotNull(future);

                yield return self.ResumeAfter(future);

                if (future.HasTimedOut)
                {
                    Warn($"{CurrentPlayerModel} timed-out");
                    yield return self.ResumeAfter(New.Coroutine(PlayerTimedOut));
                    continue;
                }
                if (!future.Available)
                    Warn($"{CurrentPlayerModel} didn't make a request");

                // do the arbitration before we test for time out
                var request = future.Value.Request as IGameRequest;
                var response = Model.Arbitrate(request);
                response.Request = request;
                future.Value.Responder?.Invoke(response);

                if (response.Failed)
                    Warn($"{request} failed for {CurrentPlayerModel}");

                var now = Kernel.Time.Now;
                var dt = (float)(now - _timeStart).TotalSeconds;

                _timeStart = now;
                _timeOut -= dt;

                if (request is TurnEnd && response.Success)
                    ResetTurnTimer();
            }
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }

        private void ResetTurnTimer()
        {
            _timeOut = Parameters.GameTurnTimer;
            _timeStart = Kernel.Time.Now;
        }

        private IEnumerator PlayerTimedOut(IGenerator arg)
        {
            Warn($"{CurrentPlayerModel} TimedOut");
            ResetTurnTimer();
            yield break;
        }

        public void EndTurn()
        {
        }

        private void TurnEnded(IResponse response)
        {
            Info($"Player {response.Request.Owner} ended turn");
        }

        private IEnumerator EndGame(IGenerator self)
        {
            Info("Game Ended");
            yield break;
        }

        /// <summary>
        /// Make a TimedBarrier that contains a collection of futures.
        /// When the barrier is completed, perform an act on each future that
        /// was in the barrier.
        /// </summary>
        private IGenerator TimedBarrierOfFutures<T>(
            TimeSpan timeOut,
            IEnumerable<IFuture<T>> futures,
            Action<IFuture<T>> act)
        {
            throw new NotImplementedException();
            //return New.TimedBarrier(timeOut, futures).ForEach(act);
        }
    }
}
