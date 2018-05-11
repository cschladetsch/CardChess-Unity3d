using System;
using System.Collections;
using System.Linq;
using App.Database;
using Flow;

using App.View;
using App.Model;
using UnityEngine.Assertions;
using IPlayer = App.Agent.IPlayer;

namespace App
{
    /// <summary>
    /// The 'umpire' of the game: enforces all the rules.
    /// </summary>
    public class Arbiter : Logger
    {
        public static IKernel Kernel;
        public IPlayer WhitePlayer => _players[0];
        public IPlayer BlackPlayer => _players[1];

        public IPlayer CurrentPlayer => _players[_currentPlayer];
        public Board Board { get; }

        public Arbiter()
        {
            Kernel = Create.Kernel();
            _new = Kernel.Factory;
        }

        public Arbiter(Board board)
            : this()
        {
            Assert.IsNotNull(board);
            Board = board;
        }

        /// <summary>
        /// Make a new Agent that represents a Model.
        /// </summary>
        public TAgent NewAgent<TAgent, TModel>(TModel model)
            where TAgent : class, ITransient, Agent.IAgent<TModel>, new()
            where TModel : class
        {
            var agent = _new.Prepare(new TAgent());
            if (!agent.Create(model))
            {
                Error("Failed to create Agent {0} for Model {1}", typeof(TAgent), typeof(TModel));
                return null;
            }

            return agent;
        }

        public void SetPlayers(IPlayer p0, IPlayer p1)
        {
            Assert.IsNotNull(p0);
            Assert.IsNotNull(p1);
            _players[0] = p0;
            _players[1] = p1;
        }

        public void NewGame()
        {
            Info("New Game");

            _currentPlayer = 0;
            _turnNumber = 0;

            foreach (var player in _players)
            {
                Assert.IsNotNull(player);
                player.NewGame();
            }

            Board.NewGame();
        }

        public void StartGame()
        {
            Info("Game Started");

            // TODO Kernel.Root.Add(_new.Coroutine(PlayerStart, WhitePlayer));
            // TODO Kernel.Root.Add(_new.Coroutine(PlayerStart, BlackPlayer));

            Kernel.Root.Add(_new.Coroutine(PlayerTurn, CurrentPlayer));
        }

        public void Step()
        {
            Kernel.Step();
        }

        IEnumerator PlayerTurn(IGenerator self, IPlayer player)
        {
            ++_turnNumber;

            Info("Start turn {0}", _turnNumber);

            if (PlayerCheckMated(player))
            {
                yield return self.ResumeAfter(PlayerLost(player));
                yield break;
            }

            player.AddMaxMana(1);

            SaveState();
            var timeOut = 60;
            var checks = 0;
            //_playerTimerCountdown = new_.Coroutine(PlayerTimerCountdownCoro, player);

        retry:
            RestoreState();
            if (checks == 3)
            {
                yield return self.ResumeAfter(PlayerLost(player));
                yield break;
            }

            var tryPlayCard = player.PlayCard();
            var tryMovePiece = player.MovePiece();
            var passed = player.Pass();

            var trigger = _new.TimedTrigger(TimeSpan.FromSeconds(timeOut), tryPlayCard, tryMovePiece, passed);
            yield return self.ResumeAfter(trigger);
            if (trigger.HasTimedOut || trigger.Reason == passed)
            {
                yield return self.ResumeAfter(PlayerTimedOut(player));
                goto next;
            }

            SaveState();
            if (trigger.Reason == tryPlayCard)
            {
                var canPlay = _new.Future<bool>();
                yield return self.ResumeAfter(TestCanPlayCard(player, tryPlayCard.Value, canPlay));
                if (canPlay.Available && canPlay.Value)
                {
                    yield return self.ResumeAfter(PerformPlayCard(tryPlayCard.Value));
                    if (InCheck(player))
                    {
                        ++checks;
                        yield return self.ResumeAfter(PlayerInCheck(player));
                        goto retry;
                    }
                    goto next;
                }
                timeOut = trigger.Timer.TimeRemaining.Seconds;
                goto retry;
            }

            if (trigger.Reason == tryMovePiece)
            {
                var canPlay = _new.Future<bool>();
                yield return self.ResumeAfter(TestCanMovePiece(player, tryMovePiece.Value, canPlay));
                if (canPlay.Available && canPlay.Value)
                {
                    yield return self.ResumeAfter(PerformMovePiece(tryMovePiece.Value));
                    if (InCheck(player))
                    {
                        ++checks;
                        yield return self.ResumeAfter(PlayerInCheck(player));
                        goto retry;
                    }
                    goto next;
                }
                timeOut = trigger.Timer.TimeRemaining.Seconds;
                goto retry;
            }

        next:
            yield return self.ResumeAfter(NextPlayerTurn());
        }

        void SaveState()
        {
            // TODO: Save state of the board, and both players' hands
            Info("SaveState");
        }

        void RestoreState()
        {
            Info("RestoreState");
            // TODO: Restore state of the board, and both players' hands
        }

        bool CanPlayCard(IPlayer player, PlayCard playCard)
        {
            // TODO: Determine if the given action is valid
            return false;
        }

        bool CanMovePiece(IPlayer player, MovePiece move)
        {
            // TODO: determine if player can perform the given move
            return false;
        }

        bool PlayerCheckMated(IPlayer player)
        {
            // This will be very hard to determine.
            // Will have to figure out all outcomes of each way the
            // player could play a card or make a move.
            return false;
        }

        bool InCheck(IPlayer player)
        {
            // TODO: calc if player's King is under attack
            return false;
        }

        // wrappers to create coroutines
        IGenerator PlayerLost(IPlayer player) { return _new.Coroutine(PlayerLostCoro, player); }
        IGenerator PlayerTimedOut(IPlayer player) { return _new.Coroutine(PlayerTimedOutCoro, player); }
        IGenerator TestCanPlayCard(IPlayer player, PlayCard play, IFuture<bool> future) { return _new.Coroutine(TestCanPlayCardCoro, play, future); }
        IGenerator TestCanMovePiece(IPlayer player, MovePiece move, IFuture<bool> future) { return _new.Coroutine(TestCanMovePieceCoro, move, future); }
        IGenerator PerformPlayCard(PlayCard playCard) { return _new.Coroutine(PlayCardCoro, playCard); }
        IGenerator PerformMovePiece(MovePiece move) { return _new.Coroutine(MovePieceCoro, move); }
        IGenerator PlayerInCheck(IPlayer player) { return _new.Coroutine(PlayerInCheckCoro, player); }
        IGenerator NextPlayerTurn() { return _new.Coroutine(NextPlayerTurnCoro); }

        IEnumerator PlayerLostCoro(IGenerator self, IPlayer loser)
        {
            // TODO: show player lost sequence
            Info("Player {0} lost", loser);
            //yield return self.ResumeAfter(_view.PlayerLost(loser));
            yield break;
        }

        IEnumerator PlayerTimedOutCoro(IGenerator self, IPlayer player)
        {
            Info("Player {0} timedout", player);
            //yield return self.ResumeAfter(_view.PlayerTimedOut(player));
            yield break;
        }

        IEnumerator TestCanPlayCardCoro(IGenerator self, PlayCard playCard, IFuture<bool> canPlay)
        {
            // TODO: if card can't be played, show why
            Info("Card play {0} is invalid");
            //yield return self.ResumeAfter(_view.InvalidPlay(playCard));
            canPlay.Value = false;
            yield break;
        }

        IEnumerator TestCanMovePieceCoro(IGenerator self, MovePiece move, IFuture<bool> canMove)
        {
            // TODO: if card can't be moved, show why
            Info("Move {0} is invalid");
            //yield return self.ResumeAfter(_view.InvalidMove(move));
            canMove.Value = false;
            yield break;
        }

        IEnumerator PlayCardCoro(IGenerator self, PlayCard playCard)
        {
            // TODO: play the card
            Info("PlayCard: {0}", playCard);
            //yield return self.ResumeAfter(_view.PlayCard(playCard));
            yield break;
        }

        IEnumerator MovePieceCoro(IGenerator self, MovePiece move)
        {
            Info("Move: {0}", move);
            //yield return self.ResumeAfter(_view.MovePiece(move));
            yield break;
        }

        IEnumerator PlayerInCheckCoro(IGenerator self, IPlayer player)
        {
            // TODO: show why player is in check
            Info("Player {0} is in check", player.Color, player);
            //yield return self.ResumeAfter(_view.PlayerInCheck(player));
            yield break;
        }

        IEnumerator NextPlayerTurnCoro(IGenerator self)
        {
            var previous = CurrentPlayer;
            _currentPlayer = (_currentPlayer + 1) % _players.Length;
            // TODO: animations etc.
            Info("Next player turn {0}", _currentPlayer);
            //yield return self.ResumeAfter(_view.NextPlayerTurn(previous, CurrentPlayer));
            yield break;
        }

        public static ICardInstance NewCard(ECardType type, IPlayer owner)
        {
            var template = _cardTemplates.OfType(type).FirstOrDefault();
            if (template == null)
                return null;
            return CardInstance.New(template, owner);
        }

        #region Private Fields

        private readonly IPlayer[] _players = new IPlayer[2];
        private int _currentPlayer;
        private IFactory _new;
        private ICoroutine _playerTimerCountdown;
        private int _turnNumber;
        private static Database.CardTemplates _cardTemplates = new CardTemplates();

        #endregion
    }
}
