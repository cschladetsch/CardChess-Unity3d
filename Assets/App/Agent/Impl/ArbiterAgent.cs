using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public void StartGame(IPlayerAgent p0, IPlayerAgent p1)
        {
            Info("StartGame");

            _players.Add(p0);
            _players.Add(p1);
            _currentPlayer = 0;

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

        public ITransient NewGame()
        {
            return null;
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

        private IEnumerator StartGame(IGenerator self)
        {
            var start = New.Sequence(
                New.Barrier(
                    _players.Select(p => p.StartGame())
                ).Named("InitGame"),
                New.Barrier(
                    _players.Select(p => p.DrawInitialCards())
                ).Named("DealCards"),
                New.Barrier(
                    New.TimedBarrier(
                        TimeSpan.FromSeconds(Parameters.MulliganTimer),
                        _players.Select(p => p.Mulligan())
                    ).Named("Mulligan"),
                    New.Barrier( // TODO: TimedOrderedNode or something...
                        _players.Select(p => p.PlaceKing())
                    ).Named("PlaceKings")
                ).Named("Preceedings")
            ).Named("Start Game");
            start.Completed += (tr) => Info("StartGameCompleted");
            yield return start;
        }

        private IEnumerator PlayerTurn(IGenerator self)
        {
            CurrentPlayer.Model.StartTurn();
            var timeOut = Parameters.GameTurnTimer;

            while (true)
            {
                var future = New.TimedFuture<IRequest>(TimeSpan.FromSeconds(timeOut));
                yield return self.After(future);

                if (future.HasTimedOut)
                {
                    Warn($"{CurrentPlayer.Model} Timed out");
                    yield return self.After(New.Coroutine(PlayerTimedOut));
                    break;
                }

                var response = Model.Arbitrate(future.Value);

                timeOut -= Kernel.Time.Delta.Seconds;
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
