using System;
using System.Collections.Generic;
using System.Linq;
using App.Action;
using App.Common;

// DI fails this inspection test
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace App.Model
{
    using Registry;

    public class ArbiterModel
        : ModelBase
        , IOwner
        , IArbiterModel
    {
        #region Public Properties

        public EGameState GameState { get; private set; }
        [Inject] public IBoardModel Board { get; set; }
        public EColor Color => EColor.Neutral;
        public IPlayerModel CurrentPlayer => _players[_currentPlayer].Player;

        #endregion

        #region Public Methods

        public ArbiterModel()
        {
            Subject = this;
            LogPrefix = "Arbiter";
        }

        public void NewGame(IPlayerModel w, IPlayerModel b)
        {
            _players = new List<PlayerEntry>()
            {
                new PlayerEntry(w),
                new PlayerEntry(b),
            };
            _currentPlayer = 0;

            Construct(this);
            Board.NewGame();
            foreach (var entry in _players)
                entry.Player.NewGame();

            GameState = EGameState.Mulligan;
        }

        public void PlayerMulligan(IPlayerModel player, IEnumerable<ICardModel> cards)
        {
            NotImplemented();
        }

        public void Endame()
        {
            Info("EndGame");
            GameState = EGameState.Completed;
        }

        private Response TryPlayCard(Action.PlayCard act)
        {
            var player = act.Player;
            var coord = act.Coord;
            var card = act.Card;

            // make the piece
            var piece = Registry.New<IPieceModel>(player, card);
            if (piece == null)
            {
                return Failed($"Player {player} couldn't make a piece using {card}");
            }

            // check for empty target square
            var existing = Board.At(coord);
            if (existing != null)
            {
                if (piece.Owner == player)
                    return TryMount(piece, existing);
                return Failed($"{player} can't play {card} when opponent {existing} already at {coord}");
            }

            return Board.TryPlacePiece(piece, coord) ? Response.Ok : Response.Fail;
        }

        public Response RequestPlayCard(Action.PlayCard act)
        {
            Assert.IsNotNull(act);
            Assert.IsNotNull(act.Player);
            Assert.IsNotNull(act.Card);

            switch (GameState)
            {
                case EGameState.PlaceKing:
                {
                    var resp = TryPlayCard(act);
                    if (resp.Type == EResponse.Ok && Board.NumPieces(EPieceType.King) == 2)
                    {
                        _currentPlayer = 0;
                        CurrentPlayer.StartTurn();
                        GameState = EGameState.PlayTurn;
                    }
                    return resp;
                }
                case EGameState.PlayTurn:
                    return TryPlayCard(act);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Response TryMovePiece(MovePiece act)
        {
            var player = act.Player;
            var coord = act.Coord;
            var piece = act.Piece;

            if (GameState != EGameState.PlayTurn)
            {
                return Failed($"Current game state is {GameState}, {player} cannot move {piece}");
            }

            if (CurrentPlayer != player)
            {
                return Failed($"Not {player}'s turn");
            }

            if (player != piece.Player)
            {
                return Failed($"No spells to allow you to move other player's pieces yet");
            }

            // check that the piece can actually move there
            if (!Board.GetMovements(piece.Coord).Contains(coord))
            {
                return Failed($"{player} cannot move {piece} to {coord}");
            }

            var existing = Board.At(coord);
            if (existing != null)
            {
                return existing.Player != player
                    ? Battle(piece, existing)
                    : TryMount(piece, existing);
            }

            return Board.TryMovePiece(piece, coord);
        }

        //Response TryTurnStart(TurnStart turnStart)
        //{
        //    if (turnStart.Player == CurrentPlayer)
        //    {
        //        GameState = EGameState.TurnPlay;
        //        return Response.Ok;
        //    }
        //    return Failed($"It is {CurrentPlayer}'s turn to start; not {turnStart.Player}'s");
        //}

        Response TryRejectCards(IRequest request)
        {
            // TODO: deal with mulligan of type RejectCards
            var entry = GetEntry(request.Player);
            if (entry.AcceptedCards)
            {
                Error($"Player {request.Player} cannot Accept twice");
                //return Response.Fail;
            }
            // TODO: give player different cards in response
            entry.AcceptedCards = true;
            if (_players.All(a => a.AcceptedCards))
                GameState = EGameState.PlaceKing;

            return Response.Ok;
        }

        public Response Arbitrate(IRequest request)
        {
            Info($"Handling {request} when in state {GameState}");
            switch (GameState)
            {
                case EGameState.None:
                    return Response.Ok;
                case EGameState.Start:
                    return Response.Ok;
                case EGameState.Mulligan:
                    return TryRejectCards(request);
                case EGameState.PlaceKing:
                    return RequestPlayCard(request as PlayCard);
                case EGameState.PlayTurn:
                    switch (request.Action)
                    {
                        case EActionType.TurnEnd:
                            return TryTurnEnd(request as TurnEnd);
                        case EActionType.Resign:
                            return TryResign(request as Resign);
                        case EActionType.CastSpell:
                            return TryCastSpell(request as CastSpell);
                        case EActionType.PlayCard:
                            return TryPlayCard(request as PlayCard);
                        case EActionType.MovePiece:
                            return TryMovePiece(request as MovePiece);
                        default:
                            NotImplemented($"{request}");
                            return Response.NotImplemented;
                    }
                case EGameState.Battle:
                    return TryBattle(request as Battle);
                case EGameState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return NotImplemented($"{request}");
        }

        Response TryResign(Resign resign)
        {
            return NotImplemented("Resignation");
        }

        Response TryBattle(Battle battle)
        {
            return NotImplemented("Battles");
        }

        Response TryTurnEnd(TurnEnd turnEnd)
        {
            Assert.IsNotNull(turnEnd);
            if (turnEnd.Player != CurrentPlayer)
            {
                Warn($"It's not {turnEnd.Player}'s turn to end");
                return Response.Fail;
            }

            CurrentPlayer.EndTurn();
            _currentPlayer = (_currentPlayer + 1) % _players.Count;
            _turnNumber++;
            CurrentPlayer.StartTurn();

            Verbose(10, $"Next turn #{_turnNumber} for {CurrentPlayer}");

            GameState = EGameState.PlayTurn;
            return Response.Ok;
        }

        Response TryCastSpell(CastSpell castSpell)
        {
            return NotImplemented($"{castSpell}");
        }

        private Response TryMount(IPieceModel piece, IPieceModel existing)
        {
            return NotImplemented("Mounting");
        }

        public Response Battle(IPieceModel attacker, IPieceModel defender)
        {
            Assert.IsNotNull(attacker);
            Assert.IsNotNull(defender);
            Assert.AreNotSame(attacker.Owner, defender.Owner);

            attacker.Attack(defender);
            defender.Respond(attacker);

            return Response.Ok;
        }

        private Response NotImplemented(string text = "")
        {
            return Failed($"Not Implemented {text}");
        }

        private Response Failed(string text = "")
        {
            Warn(text);
            return Response.Fail;
        }

        private PlayerEntry GetEntry(IPlayerModel player)
        {
            Assert.IsNotNull(player);
            Assert.IsTrue(_players.Any(p => p.Player == player));
            return _players.First(p => p.Player == player);
        }

        #endregion

        class PlayerEntry
        {
            public bool AcceptedCards;
            public readonly IPlayerModel Player;

            public PlayerEntry(IPlayerModel player)
            {
                Player = player;
            }
        }

        #region Private Fields

        private List<PlayerEntry> _players;
        private int _currentPlayer;
        private int _turnNumber;

        #endregion
    }
}
