using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoLib;
using Flow;

namespace App
{
    using Common;
    using Common.Message;
    using Agent;
    using Model;
    using Registry;

    /// <summary>
    /// The 'adudicator' of the game: controls the sequencing of the events
    /// but not all the rules.
    /// </summary>
    public class ArbiterAgent
        : AgentBaseCoro<IArbiterModel>
        , IArbiterAgent
    {
        public IPlayerAgent CurrentPlayer => _players[_currentPlayer];

        public ArbiterAgent(IArbiterModel model)
            : base(model)
        {
        }

        public void Step()
        {
            Info($"Step: {Kernel.StepNumber}");
            Kernel.Step();
        }

        public ITransient NewGame()
        {
            return null;
        }

        public void StartGame(IPlayerAgent p0, IPlayerAgent p1)
        {
            Info("StartGame");

            _players.Add(p0);
            _players.Add(p1);
            _currentPlayer = 0;

            Model.NewGame(p0.Model, p1.Model);

            _Node.Add(
                New.Sequence(
                    New.Barrier(
                        NewGame(),
                        Board.NewGame()
                    ).Named("Setup"),
                    New.Barrier(
                        _players.Select(p => p.StartGame())
                    ).Named("PlayersStartGame")
                ).Named("Setup"),
                GameLoop()
            );
        }

        public ITransient GameLoop()
        {
            return New.Sequence(
                New.Coroutine(StartGame),
                New.While(() => Model.GameState.Value != EGameState.Completed,
                    New.Coroutine(PlayerTurn).Named("Turn")
                ).Named("While"),
                New.Coroutine(EndGame).Named("EndGame")
            ).Named("GameLoop");
        }

        IGenerator TimedFutureBarrier<T>(
            TimeSpan span,
            IEnumerable<IFuture<T>> futures,
            Action<IFuture<T>> act)
        {
            return New.TimedBarrier(span, futures).ForEach(act);
        }

        private IEnumerator StartGame(IGenerator self)
        {
            var rejectTime = TimeSpan.FromSeconds(Parameters.MulliganTimer);
            var kingPlaceTime = TimeSpan.FromSeconds(Parameters.PlaceKingTimer);

            var start = New.Barrier(
                New.Sequence(
                    New.Barrier(
                        _players.Select(p => p.StartGame())
                    ).Named("InitGame"),
                    New.Barrier(
                        _players.Select(p => p.DrawInitialCards())
                    ).Named("DealCards")
                ),
                TimedFutureBarrier(
                    rejectTime,
                    _players.Select(p => p.Mulligan()),
                    f => { if (f.Available) Model.Arbitrate(f.Value); }
                ).Named("Mulligan"),
                TimedFutureBarrier(
                    kingPlaceTime,
                    _players.Select(p => p.PlaceKing()),
                    f => { if (f.Available) Model.Arbitrate(f.Value); }
                ).Named("PlaceKings")
            ).Named("StartGame");
            yield return start;
        }

        private IEnumerator PlayerTurn(IGenerator self)
        {
            CurrentPlayer.Model.StartTurn();
            var timeOut = Parameters.GameTurnTimer;

            while (true)
            {
                var future = CurrentPlayer.NextRequest();
                if (future == null)
                    break;

                yield return self.After(future);

                //if (future.HasTimedOut)
                //{
                //    Warn($"{CurrentPlayer.Model} Timed out");
                //    yield return self.After(New.Coroutine(PlayerTimedOut));
                //    break;
                //}

                var response = Model.Arbitrate(future.Value);

                timeOut -= Kernel.Time.Delta.Seconds;

                break;
            }

            _currentPlayer = (_currentPlayer + 1) % _players.Count;
        }

        private IEnumerator PlayerTimedOut(IGenerator arg)
        {
            Warn($"{CurrentPlayer.Model} TimedOut");
            yield break;
        }

        private IEnumerator EndGame(IGenerator self)
        {
            Info("Game Ended");
            yield break;
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

        readonly List<IPlayerAgent> _players = new List<IPlayerAgent>();
        private int _currentPlayer = 0;
    }
}
