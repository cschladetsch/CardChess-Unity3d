using System;
using System.Collections;
using System.Linq;
#if UNITY_2018
using UnityEngine.Assertions;
#else
#endif

using Flow;
using UniRx;

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
        : AgentBase<IArbiterModel>
        , IArbiterAgent
    {
        #region Public Fields
        public IPlayerAgent WhitePlayer => _players[0];
        public IPlayerAgent BlackPlayer => _players[1];
        public IPlayerAgent CurrentPlayerAgent => _players[_currentPlayer];

        public bool Destroyed { get; private set; }
        public Guid Id { get; set; }
        public IRegistry<IAgent> Registry { get; set; }
        #endregion

        public void StartGame(IPlayerAgent p0, IPlayerAgent p1)
        {
            Info("StartGame");

            _players[0] = p0;
            _players[1] = p1;

            _currentPlayer = 0;
            _turnNumber = 0;

            Board.NewGame();
            foreach (var player in _players)
            {
                Assert.IsNotNull(player);
                player.NewGame();
            }

            GameLoop();
        }

        public void Step()
        {
            Info($"Step: {Kernel.StepNumber}");
            Kernel.Step();
        }

        /*
        public TModel NewModel<TModel>()
            where TModel : class, IConstructWith, new()
        {
            var model = new TModel();
            if (!model.Construct())
            {
                Error("Failed to create Model {0}", typeof(TModel));
                return null;
            }
            return model;
        }

        public TModel NewModel<TModel, A0>(A0 a0)
            where TModel : class, IConstructWith<A0>, new()
        {
            var model = new TModel();
            if (!model.Construct(a0))
            {
                Error("Failed to create Model {0} with arg {1}", typeof(TModel), a0);
                return null;
            }
            return model;
        }

        public TModel NewModel<TModel, A0, A1>(A0 a0, A1 a1)
            where TModel : class, IConstructWith<A0, A1>, new()
        {
            var model = new TModel();
            if (!model.Construct(a0, a1))
            {
                Error("Failed to create Model {0} with args {1}, {2}", typeof(TModel), a0, a1);
                return null;
            }

            return model;
        }

        /// <summary>
        /// Make a new Agent that represents a
        /// </summary>
        public TAgent NewAgent<TAgent, TModel>(TModel model)
            where TModel : class, Model.IModel
            where TAgent : class, IAgent<TModel>, new()
        {
            var agent = new TAgent();
            if (!agent.Construct(model))
            {
                Error("Failed to create Agent {0} for Model {1}", typeof(TAgent), typeof(TModel));
                return null;
            }
            agent.Arbiter = this;

            return agent;
        }

        public ICardModel NewCardModel(ICardTemplate tmpl, IOwner owner)
        {
            return new CardModel(tmpl, owner);
        }

        public ICardAgent NewCardAgent(ICardTemplate template, IOwner owner)
        {
            //var cardInstance = Database.CardTemplates.NewCardModel(Registry, template.Id, owner);
            // TODOreturn NewAgent<Card, ICard>(cardInstance);
            return null;
        }

        public ICardAgent NewCardAgent(ICardModel model, IOwner owner)
            => NewCardAgent(model.Template, owner);

        public ICardAgent NewCardAgent(EPieceType type, IOwner owner)
        {
            var template = Database.CardTemplates.OfType(type).FirstOrDefault();
            return template == null ? null : NewCardAgent(template, owner);
        }

        public IEntity<TModel, TAgent> NewEntity<TModel, A0, A1, TAgent>(A0 a0, A1 a1)
            where TModel : class, IModel, IConstructWith<A0, A1>, new()
            where TAgent : class, IAgent<TModel>, new()
        {
            var model = NewModel<TModel, A0, A1>(a0, a1);
            var agent = NewAgent<TAgent, TModel>(model);
            var entity = new Entity<TModel, TAgent>();
            entity.Construct(model, agent);
            return entity;
        }

        #endregion
        #endregion
        */


        #region Private Methods
        public void GameLoop()
        {
            Root.Add(
                New.Sequence(
                    New.Coroutine(StartGame).Named("StartGame"),
                    New.While(() => !_gameOver),
                        New.Coroutine(PlayerTurn).Named("Turn").Named("While"),
                    New.Coroutine(EndGame).Named("EndGame")
                ).Named("GameLoop")
            );
        }

        private IEnumerator StartGame(IGenerator self)
        {
            var start = New.Sequence(
                New.Barrier(
                    WhitePlayer.StartGame(),
                    BlackPlayer.StartGame()
                ).Named("Init Game"),
                New.Barrier(
                    WhitePlayer.DrawInitialCards(),
                    BlackPlayer.DrawInitialCards()
                ).Named("Deal Cards"),
                New.Barrier(
                    New.TimedBarrier(
                        TimeSpan.FromSeconds(Parameters.MulliganTimer),
                        WhitePlayer.AcceptCards(),
                        BlackPlayer.AcceptCards()
                    ).Named("Mulligan"),
                    New.Sequence( // TODO: TimedSequence
                        WhitePlayer.PlaceKing(),
                        BlackPlayer.PlaceKing()
                    ).Named("Place Kings")
                ).Named("Preceedings")
            ).Named("Start Game");
            start.Completed += (tr) => Info("StartGame completed");
            yield return start;
        }

        private IEnumerator PlayerTurn(IGenerator self)
        {
            ++_turnNumber;
            var player = CurrentPlayerAgent;

            Info($"Start turn {_turnNumber} for {player}");

            yield return self.After(
                New.Barrier(
                    player.ChangeMaxMana(1).Named("AddMaxMana"),
                    player.DrawCard().Named("DrawCard")
                ).Named("Upkeep")
            );

            var timeOut = Parameters.GameTurnTimer;

            while (true)
            {
                // the options a playerAgent has
                var playCard = player.PlayCard();
                var movePiece = player.MovePiece();
                var pass = player.Pass();
                var trigger = New.TimedTrigger(
                    TimeSpan.FromSeconds(timeOut),
                    playCard,
                    movePiece,
                    pass
                );
                trigger.Name = "PlayerAgent Options";

                // wait for playerAgent to make a move
                yield return self.After(trigger);

                // timed out, next playerAgent's turn
                if (trigger.HasTimedOut || trigger.Reason == pass)
                {
                    Warn($"PlayerAgent {CurrentPlayerAgent} Timed out");
                    yield return self.After(PlayerTimedOut(player));
                    break;
                }

                // true if a valid play - move piece or play card
                var canPlay = New.Future<bool>();

                // a card was played
                if (trigger.Reason == playCard)
                {
                    yield return self.After(TestCanPlayCard(player, playCard.Value, canPlay));
                    if (canPlay.Available && canPlay.Value)
                    {
                        yield return self.After(PerformPlayCard(playCard.Value));
                        break;
                    }
                    if (canPlay.Available && playCard.Available)
                        Warn($"{playCard.Value} cannot be played");
                    continue;
                }

                // a piece was moved
                if (trigger.Reason == movePiece)
                {
                    yield return self.After(TestCanMovePiece(player, movePiece.Value, canPlay));
                    if (canPlay.Available && canPlay.Value)
                    {
                        yield return self.After(PerformMovePiece(movePiece.Value));
                        break;
                    }
                    if (canPlay.Available && movePiece.Available)
                        Warn($"{movePiece.Value} cannot be played");
                    continue;
                }
            }

            _gameOver = CurrentPlayerAgent.Health.Value <= 0;
            _currentPlayer = (_currentPlayer + 1) % 2;
        }
        private IEnumerator EndGame(IGenerator self)
        {
            Info("Game Ended");
            yield break;
        }

        // wrappers to create coroutines
        private IGenerator PlayerTimedOut(IPlayerAgent playerAgent) { return New.Coroutine(PlayerTimedOutCoro, playerAgent); }
        private IGenerator TestCanPlayCard(IPlayerAgent playerAgent, PlacePiece play, IFuture<bool> future) { return New.Coroutine(TestCanPlayCardCoro, playerAgent, play, future); }
        private IGenerator TestCanMovePiece(IPlayerAgent playerAgent, MovePiece move, IFuture<bool> future) { return New.Coroutine(TestCanMovePieceCoro, playerAgent, move, future); }
        private IGenerator PerformPlayCard(PlacePiece placePiece) { return New.Coroutine(PlayCardCoro, placePiece); }
        private IGenerator PerformMovePiece(MovePiece move) { return New.Coroutine(MovePieceCoro, move); }
        private IEnumerator PlayerLostCoro(IGenerator self, IPlayerAgent loser)
        {
            // TODO: show playerAgent lost sequence
            Info($"PlayerAgent {loser.Color} lost");
            //yield return self.After(_view.PlayerLost(loser));
            yield break;
        }
        private IEnumerator PlayerTimedOutCoro(IGenerator self, IPlayerAgent playerAgent)
        {
            Info($"PlayerAgent {playerAgent.Color} timedout");
            //yield return self.After(_view.PlayerTimedOut(playerAgent));
            yield break;
        }
        private IEnumerator TestCanPlayCardCoro(IGenerator self, IPlayerAgent playerAgent, PlacePiece placePiece, IFuture<bool> canPlay)
        {
            var current = Board.At(placePiece.Coord);
            if (current == null)
            {
                Info($"{placePiece} for playerAgent {playerAgent.Color} is VALID");
                canPlay.Value = true;
                yield break;
            }

            Info("${playCard} for playerAgent {playerColor} is INVALID");
            canPlay.Value = false;
        }
        private IEnumerator TestCanMovePieceCoro(IGenerator self, IPlayerAgent playerAgent, MovePiece move, IFuture<bool> canMove)
        {
            // TODO: if card can't be moved, show why
            Info($"Move {move} is invalid");
            //yield return self.After(_view.InvalidMove(move));
            canMove.Value = false;
            yield break;
        }
        private IEnumerator PlayCardCoro(IGenerator self, PlacePiece placePiece)
        {
            // TODO: play the card
            Info($"PlacePiece: {placePiece}");
            //yield return self.After(_view.PlacePiece(playCard));
            yield break;
        }
        private IEnumerator MovePieceCoro(IGenerator self, MovePiece move)
        {
            Info($"Move: {move}");
            //yield return self.After(_view.MovePiece(move));
            yield break;
        }

        public void Destroy()
        {
            TransientCompleted();
        }
        #endregion

        #region Private Fields
        private readonly IPlayerAgent[] _players = new IPlayerAgent[2];
        private int _currentPlayer;
        private ICoroutine _playerTimerCountdown;
        private int _turnNumber;
        private bool _gameOver;
        //private Observable.

        #endregion
    }
}
