using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using App.Model;
using Flow;
using JetBrains.Annotations;

namespace App
{
    public enum EColor { White, Black }

    public class Arbiter : Agent
    {
        public static Flow.IKernel Kernel;

        private void Awake()
        {
            Kernel = Flow.Create.Kernel();
        }

        private void Update()
        {
            Kernel.Step();
        }

        public interface IAction
        {
            IPlayer Player { get; }
        }

        public class ActionBase : IAction
        {
            public IPlayer Player { get; private set; }
        }

        public class PlayCard : ActionBase
        {
            public ICard Card;
            public Coord Coord;
        }

        public class MovePiece : ActionBase
        {
            public IInstance Instance;
            public Coord Target;
        }

        IEnumerator PlayerTurn(IGenerator self, IPlayer player)
        {
            player.AddMaxMana(1);

            SaveState();
            var timeOut = 60;
            var checks = 0;

        retry:
            RestoreState();
            if (checks == 3)
            {
                yield return self.ResumeAfter(PlayerLost(player));
                yield break;
            }

            var tryPlayCard = player.TryPlayCard();
            var tryMovePiece = player.TryMovePiece();
            var passed = player.Pass();

            var trigger = Kernel.Factory.TimedTrigger(TimeSpan.FromSeconds(timeOut), tryPlayCard, tryMovePiece, passed);
            yield return self.ResumeAfter(trigger);
            if (trigger.HasTimedOut || trigger.Reason == passed)
                goto next;

            SaveState();
            if (trigger.Reason == tryPlayCard)
            {
                IFuture<bool> canPlay = _make.Future<bool>();
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
                IFuture<bool> canPlay = _make.Future<bool>();
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
            yield return NextPlayerTurn();
        }

        void SaveState()
        {
        }

        void RestoreState()
        {
        }

        bool CanPlayCard(IPlayer player, PlayCard playCard)
        {
            return false;
        }

        bool CanMovePiece(IPlayer player, MovePiece move)
        {
            // TODO: determine if player can perform the given move
            return false;
        }

        bool InCheck(IPlayer player)
        {
            // TODO: calc if player's King is under attack
            return false;
        }

        IGenerator PlayerLost(IPlayer player) { return _make.Coroutine(PlayerLostCoro, player); }
        IGenerator TestCanPlayCard(IPlayer player, PlayCard play, IFuture<bool> future) { return _make.Coroutine(TestCanPlayCardCoro, play, future); }
        IGenerator TestCanMovePiece(IPlayer player, MovePiece move, IFuture<bool> future) { return _make.Coroutine(TestCanMovePieceCoro, move, future); } 
        IGenerator PerformPlayCard(PlayCard playCard) { return _make.Coroutine(PlayCardCoro, playCard); }
        IGenerator PerformMovePiece(MovePiece move) { return _make.Coroutine(MovePieceCoro, move); }
        IGenerator PlayerInCheck(IPlayer player) { return _make.Coroutine(PlayerInCheckCoro, player); }
        IGenerator NextPlayerTurn() { return _make.Coroutine(NextPlayerTurnCoro); }

        IEnumerator PlayerLostCoro(IGenerator self, IPlayer loser)
        {
            yield break;
        }

        IEnumerator TestCanPlayCardCoro(IGenerator self, PlayCard playCard, IFuture<bool> canPlay)
        {
            // TODO: if card can't be played, show why
            yield break;
        }

        IEnumerator TestCanMovePieceCoro(IGenerator self, MovePiece move, IFuture<bool> can)
        {
            // TODO: if card can't be moved, show why
            yield break;
        }

        IEnumerator PlayCardCoro(IGenerator self, PlayCard playCard)
        {
            // TODO: play the card
            Info("PlayCard: {0}", playCard);
            yield break;
        }

        IEnumerator MovePieceCoro(IGenerator self, MovePiece move)
        {
            // TODO: move piece
            Info("Move: {0}", move);
            yield break;
        }

        IEnumerator PlayerInCheckCoro(IGenerator self, IPlayer player)
        {
            // TODO: show why player is in check
            Info("Player {0} is in check", player.Color, player);
            yield break;
        }

        IEnumerator NextPlayerTurnCoro(IGenerator self)
        {
            _currentPlayer = (_currentPlayer + 1) % _players.Length;
            // TODO: animations etc.
            Info("Next player turn {0}", _currentPlayer);
            yield break;
        }

        private IPlayer[] _players;
        private int _currentPlayer;
        private IFactory _make;
    }
}
