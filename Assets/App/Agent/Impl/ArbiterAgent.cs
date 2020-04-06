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
            Verbose(50, $"{this} created");
        }

        public void Step()
        {
            Verbose(20, $"Step: {Kernel.StepNumber}");
            Kernel.Step();
        }

        public ITransient PrepareGame(IPlayerAgent p0, IPlayerAgent p1)
        {
            Assert.IsNotNull(p0);
            Assert.IsNotNull(p1);

            Model.PrepareGame(p0.Model, p1.Model);
            _playerAgents = new List<IPlayerAgent> {p0, p1};
            Model.CurrentPlayer.Subscribe(SetPlayerAgent);

            // TODO: do some animations etc
            return null;
        }

        private void SetPlayerAgent(IPlayerModel model)
        {
            foreach (var p in _playerAgents)
            {
                if (p.Model != model)
                    continue;

                _playerAgent.Value = p;
                return;
            }
            throw new Exception("Player agent not found");
        }

        public override void StartGame()
        {
            Info($"{this} StartGame");

            BoardAgent.StartGame();

            foreach (var p in _playerAgents)
                p.StartGame();

            ITransient GameLoop() => New.Coroutine(PlayerTurn).Named("Turn");
            _Node.Add(GameLoop());
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
