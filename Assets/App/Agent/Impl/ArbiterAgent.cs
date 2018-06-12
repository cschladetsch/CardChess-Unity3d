using System;
using System.Collections;
using System.Collections.Generic;

using Flow;
using UniRx;

namespace App
{
    using Common.Message;
    using Common;
    using Registry;
    using Agent;
    using Model;

    /// <summary>
    /// The 'Adjudicator' of the game: controls the sequencing of the events
    /// but not all the rules.
    ///
    /// Responsiblity for enforcing the rules of the game are shared with
    /// the Board- and Card-Agents and Models.
    /// </summary>
    public class ArbiterAgent
        : AgentBaseCoro<IArbiterModel>
        , IArbiterAgent
    {
        public IReadOnlyReactiveProperty<IPlayerAgent> PlayerAgent => _playerAgent;
        [Inject] public IBoardAgent BoardAgent { get; set; }
        public IPlayerAgent WhitePlayerAgent => _playerAgents[0];
        public IPlayerAgent BlackPlayerAgent => _playerAgents[1];
        public IReadOnlyReactiveProperty<EGameState> GameState => Model.GameState;
        public IPlayerModel CurrentPlayerModel => PlayerAgent.Value.Model;

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

        void SetPlayerAgent(IPlayerModel model)
        {
            foreach (var p in _playerAgents)
            {
                if (p.Model == model)
                {
                    _playerAgent.Value = p;
                    return;
                }
            }
            throw new Exception("Player agent not found");
        }

        public override void StartGame()
        {
            Info($"{this} StartGame");

            Board.StartGame();

            // only needed because we're skipping the coro below
            foreach (var p in _playerAgents)
                p.StartGame();

            _Node.Add(GameLoop());
            //_Node.Add(GameLoop());
            //_Node.Add(
            //    New.Sequence(
            //        New.Barrier(
            //            NewGameWork(),
            //            Board.NewGameAction()
            //        ).Named("NewGame"),
            //        New.Barrier(
            //            _players.Select(p => p.StartGame())
            //        ).Named("PlayersStartGame")
            //    ).Named("Setup"),
            //    New.Log("Entering main game loop..."),
            //    GameLoop()
            //);
        }

        //private IGenerator StartGameCoro()
        //{
        //    var rejectTimeOut = TimeSpan.FromSeconds(Parameters.MulliganTimer);
        //    //var kingPlaceTimeOut = TimeSpan.FromSeconds(Parameters.PlaceKingTimer);

        //    //return New.Barrier(
        //    //    New.Sequence(
        //    //        New.Barrier(
        //    //            _playerAgents.Select(p => p.StartGame())
        //    //        ).Named("InitGame"),
        //    //        New.Barrier(
        //    //            _playerAgents.Select(p => p.DrawInitialCards())
        //    //        ).Named("DealCards")
        //    //    ),
        //    //    ArbitrateFutures(
        //    //        rejectTimeOut,
        //    //        _playerAgents.Select(p => p.Mulligan())
        //    //    ).Named("Mulligan")
        //    //).Named("StartGame");
        //}

        public ITransient GameLoop()
        {
            return New.Coroutine(PlayerTurn).Named("Turn");

            //return New.Sequence(
            //    //StartGameCoro(),
            //    New.Coroutine(PlayerTurn).Named("Turn"),
            //    New.Coroutine(EndGame).Named("EndGame")
            //).Named("Done");
        }

        private IEnumerator PlayerTurn(IGenerator self)
        {
            CurrentPlayerModel.StartTurn();

            var timeOut = Parameters.GameTurnTimer;
            var timeStart = Kernel.Time.Now;

            // player can make as many valid actions as he can during his turn
            while (true)
            {
                if (GameState.Value == EGameState.Completed)
                {
                    self.Complete();
                    yield break;
                }

                Assert.IsTrue(self.Active);

                var future = PlayerAgent.Value.NextRequest(timeOut);
                Assert.IsNotNull(future);

                yield return self.After(future);

                if (future.HasTimedOut)
                {
                    Warn($"{CurrentPlayerModel} timed-out");
                    yield return self.After(New.Coroutine(PlayerTimedOut));
                    break;
                }
                if (!future.Available)
                    Warn($"{CurrentPlayerModel} didn't make a request");

                // do the arbitration before we test for time out
                var response = Model.Arbitrate(future.Value.Request);
                response.Request = future.Value.Request;
                future.Value.Responder?.Invoke(response);
                if (response.Failed)
                    Warn($"Request {future.Value.Request} failed for {CurrentPlayerModel}");

                var now = Kernel.Time.Now;
                var dt = (float)(now - timeStart).TotalSeconds;
                //if (dt < 1.0f/60.0f)    // give them a 60Hz frame of grace
                //{
                //    Warn($"{CurrentPlayerModel} ran out of time for turn");
                //    break;
                //}

                timeStart = now;
                timeOut -= dt;
            }
        }

        private IEnumerator PlayerTimedOut(IGenerator arg)
        {
            Warn($"{CurrentPlayerModel} TimedOut");
            Model.EndTurn();
            yield break;
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
            return New.TimedBarrier(timeOut, futures).ForEach(act);
        }

        /// <summary>
        /// Make a TimedBarrier that contains a collection of future IRequests.
        /// When the barrier is completed, pass the value of each available request to
        /// the Arbiter.
        /// </summary>
        private IGenerator ArbitrateFutures<T>(
            TimeSpan timeOut,
            IEnumerable<IFuture<T>> futures,
            Action<IFuture<T>> onUnavailable = null)
            where T : IRequest
        {
            return TimedBarrierOfFutures(
                timeOut,
                futures,
                f =>
                {
                    if (f.Available)
                        Model.Arbitrate(f.Value);
                    else
                        onUnavailable?.Invoke(f);
                }
            );
        }

        //protected override IEnumerator Next(IGenerator self)
        //{
        //    // TODO: general game backround animation, music loops etc
        //    yield return null;
        //}

        private List<IPlayerAgent> _playerAgents = new List<IPlayerAgent>();
        private readonly ReactiveProperty<IPlayerAgent> _playerAgent = new ReactiveProperty<IPlayerAgent>();
    }
}
