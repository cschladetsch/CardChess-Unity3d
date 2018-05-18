using System;
using System.Collections;
using System.Linq;
#if UNITY_2018
using UnityEngine.Assertions;
#else
#endif

using Flow;

namespace App
{
    using Common;
    using Agent;
    using Model;
    using Action;

    /// <inheritdoc />
    /// <summary>
    /// The 'adudicator' of the game: controls the sequencing of the events
    /// but not all the rules.
    /// </summary>
    public class Arbiter : Flow.Impl.Logger
    {
        #region Public Fields
        public static Arbiter Instance;
        public static IKernel Kernel;
        public static INode Root => Kernel.Root;
        public static IFactory New => Kernel.Factory;
        public IPlayerAgent WhitePlayerAgent => _playersAgent[0];
        public IPlayerAgent BlackPlayerAgent => _playersAgent[1];
        public IPlayerAgent CurrentPlayerAgent => _playersAgent[_currentPlayer];
        public IBoardModel BoardModel => _boardModel;
        #endregion

        #region Public Methods
        public bool CanPlaceKing(PlayerAgent playerAgent, Coord coord)
        {
            //if (!BoardAt(coord) != null)
            //    return false;

            //foreach (var adj in BoardAdjacentTo(coord))
            //{
            //    adj.Color
            //}
            throw new NotImplementedException();

        }

        public Arbiter()
        {
            Instance = this;
            Kernel = Create.Kernel();
        }

        public void Setup(IBoardAgent boardAgent, IPlayerAgent p0, IPlayerAgent p1)
        {
            _boardAgent = boardAgent;
            SetPlayers(p0, p1);
        }

        public void SetPlayers(IPlayerAgent p0, IPlayerAgent p1)
        {
            Assert.IsNotNull(p0);
            Assert.IsNotNull(p1);
            _playersAgent[0] = p0;
            _playersAgent[1] = p1;
        }

        public void NewGame()
        {
            Assert.IsNotNull(_boardAgent);
            Assert.IsNotNull(WhitePlayerAgent);
            Assert.IsNotNull(BlackPlayerAgent);

            Info("New Game");

            _currentPlayer = 0;
            _turnNumber = 0;

            _boardAgent.NewGame();
            foreach (var player in _playersAgent)
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

        #region Creation Methods
        public TModel NewModel<TModel>()
            where TModel : class, ICreateWith, new()
        {
            var model = new TModel();
            if (!model.Create())
            {
                Error("Failed to create Model {0}", typeof(TModel));
                return null;
            }
            return model;
        }

        public TModel NewModel<TModel, A0>(A0 a0)
            where TModel : class, ICreateWith<A0>, new()
        {
            var model = new TModel();
            if (!model.Create(a0))
            {
                Error("Failed to create Model {0} with arg {1}", typeof(TModel), a0);
                return null;
            }
            return model;
        }

        public TModel NewModel<TModel, A0, A1>(A0 a0, A1 a1)
            where TModel : class, ICreateWith<A0, A1>, new()
        {
            var model = new TModel();
            if (!model.Create(a0, a1))
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
            if (!agent.Create(model))
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
            var cardInstance = Database.CardTemplates.New(template.Id, owner);
            // TODOreturn NewAgent<Card, ICard>(cardInstance);
            return null;
        }

        public ICardAgent NewCardAgent(ICardModel model, IOwner owner)
            => NewCardAgent(model.ModelTemplate, owner);

        public ICardAgent NewCardAgent(ECardType type, IOwner owner)
        {
            var template = Database.CardTemplates.OfType(type).FirstOrDefault();
            return template == null ? null : NewCardAgent(template, owner);
        }

        public IEntity<TModel, TAgent> NewEntity<TModel, A0, A1, TAgent>(A0 a0, A1 a1)
            where TModel : class, IModel, ICreateWith<A0, A1>, new()
            where TAgent : class, IAgent<TModel>, new()
        {
            var model = NewModel<TModel, A0, A1>(a0, a1);
            var agent = NewAgent<TAgent, TModel>(model);
            var entity = new Entity<TModel, TAgent>();
            entity.Create(model, agent);
            return entity;
        }

        #endregion
        #endregion

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
                    WhitePlayerAgent.StartGame(),
                    BlackPlayerAgent.StartGame()
                ).Named("Init Game"),
                New.Barrier(
                    WhitePlayerAgent.DrawInitialCards(),
                    BlackPlayerAgent.DrawInitialCards()
                ).Named("Deal Cards"),
                New.Barrier(
                    New.TimedBarrier(
                        TimeSpan.FromSeconds(Parameters.MulliganTimer),
                        WhitePlayerAgent.FutureAcceptCards(),
                        BlackPlayerAgent.FutureAcceptCards()
                    ).Named("Mulligan"),
                    New.Sequence( // TODO: TimedSequence
                        WhitePlayerAgent.FuturePlaceKing(),
                        BlackPlayerAgent.FuturePlaceKing()
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
                    player.FutureDrawCard().Named("DrawCard")
                )
            );

            var timeOut = Parameters.GameTurnTimer;

            while (true)
            {
                // the options a playerAgent has
                var playCard = player.FuturePlayCard();
                var movePiece = player.FutureMovePiece();
                var pass = player.FuturePass();
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

            _gameOver = CurrentPlayerAgent.Health <= 0;
            _currentPlayer = (_currentPlayer + 1) % 2;
        }
        private IEnumerator EndGame(IGenerator self)
        {
            Info("Game Ended");
            yield break;
        }

        // wrappers to create coroutines
        private IGenerator PlayerTimedOut(IPlayerAgent playerAgent) { return New.Coroutine(PlayerTimedOutCoro, playerAgent); }
        private IGenerator TestCanPlayCard(IPlayerAgent playerAgent, PlayCard play, IFuture<bool> future) { return New.Coroutine(TestCanPlayCardCoro, playerAgent, play, future); }
        private IGenerator TestCanMovePiece(IPlayerAgent playerAgent, MovePiece move, IFuture<bool> future) { return New.Coroutine(TestCanMovePieceCoro, playerAgent, move, future); }
        private IGenerator PerformPlayCard(PlayCard playCard) { return New.Coroutine(PlayCardCoro, playCard); }
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
        private IEnumerator TestCanPlayCardCoro(IGenerator self, IPlayerAgent playerAgent, PlayCard playCard, IFuture<bool> canPlay)
        {
            var current = BoardModel.At(playCard.Coord);
            if (current == null)
            {
                Info($"{playCard} for playerAgent {playerAgent.Color} is VALID");
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
        private IEnumerator PlayCardCoro(IGenerator self, PlayCard playCard)
        {
            // TODO: play the card
            Info($"PlayCard: {playCard}");
            //yield return self.After(_view.PlayCard(playCard));
            yield break;
        }
        private IEnumerator MovePieceCoro(IGenerator self, MovePiece move)
        {
            Info($"Move: {move}");
            //yield return self.After(_view.MovePiece(move));
            yield break;
        }
        #endregion

        #region Private Fields
        private readonly IPlayerAgent[] _playersAgent = new IPlayerAgent[2];
        private Model.BoardModel _boardModel;
        private IBoardAgent _boardAgent;
        private int _currentPlayer;
        private ICoroutine _playerTimerCountdown;
        private int _turnNumber;
        private bool _gameOver;
        #endregion
    }
}
