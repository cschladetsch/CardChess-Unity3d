using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Message;
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
            LogSubject = this;
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

        public void Endame()
        {
            Info("EndGame");
            GameState = EGameState.Completed;
        }

        private Response TryPlacePiece(PlacePiece act)
        {
            var player = act.Player;
            var coord = act.Coord;
            var card = act.Card;

            // make the piece
            var piece = Registry.New<IPieceModel>(player, card);
            if (piece == null)
            {
                return Failed(act, $"Player {player} couldn't make a piece using {card}");
            }

            // check for empty target square
            var existing = Board.At(coord);
            if (existing != null)
            {
                return Failed(act, $"{player} can't play {card} when {existing} already at {coord}");
            }

            return !Board.TryPlacePiece(piece, coord) ? Failed(act) : Response.Ok;
        }

        public Response TryPlaceKing(PlacePiece act)
        {
            Assert.IsNotNull(act);
            Assert.IsNotNull(act.Player);
            Assert.IsNotNull(act.Card);
            Assert.AreEqual(EGameState.PlaceKing, GameState);
            Assert.AreEqual(act.Card.PieceType, EPieceType.King);

            var resp = TryPlacePiece(act);
            if (resp.Type == EResponse.Ok && Board.NumPieces(EPieceType.King) == 2)
            {
                _currentPlayer = 0;
                CurrentPlayer.StartTurn();
                GameState = EGameState.PlayTurn;
            }

            return Response.Ok;
        }

        Response TryMovePiece(MovePiece act)
        {
            var player = act.Player;
            var coord = act.Coord;
            var piece = act.Piece;

            if (GameState != EGameState.PlayTurn)
            {
                return Failed(act, $"Currently in {GameState}, {player} cannot move {piece}");
            }

            if (CurrentPlayer != player)
            {
                return Failed(act, $"Not {player}'s turn");
            }

            if (player != piece.Player)
            {
                return Failed(act, $"No spells to allow you to move other player's pieces yet");
            }

            // check that the piece can actually move there
            if (!Board.GetMovements(piece.Coord).Contains(coord))
            {
                return Failed(act, $"{player} cannot move {piece} to {coord}");
            }

            var existing = Board.At(coord);
            if (existing != null)
            {
                return Failed(act, $"{coord} is occupied by {existing}");
            }

            var moved = Board.TryMovePiece(piece, coord);
            if (moved.Failed)
                Failed(act);

            return moved;
        }

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
            Info($"{request} in {GameState}");
            switch (GameState)
            {
                case EGameState.None:
                    return Response.Ok;
                case EGameState.Start:
                    return Response.Ok;
                case EGameState.Mulligan:
                    return TryRejectCards(request);
                case EGameState.PlaceKing:
                    return TryPlaceKing(request as PlacePiece);
                case EGameState.PlayTurn:
                    switch (request.Action)
                    {
                        case EActionType.TurnEnd:
                            return TryTurnEnd(request as TurnEnd);
                        case EActionType.Resign:
                            return TryResign(request as Resign);
                        case EActionType.CastSpell:
                            return TryCastSpell(request as CastSpell);
                        case EActionType.PlacePiece:
                            return TryPlacePiece(request as PlacePiece);
                        case EActionType.Battle:
                            return TryBattle(request as Battle);
                        case EActionType.MovePiece:
                            return TryMovePiece(request as MovePiece);
                        default:
                            return NotImplemented(request, $"{request}");
                    }
                case EGameState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return NotImplemented(request);
        }

        Response TryResign(Resign resign)
        {
            return NotImplemented(resign);
        }

        Response TryBattle(Battle battle)
        {
            return NotImplemented(battle);
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
            return NotImplemented(castSpell, $"{castSpell}");
        }

        public Response Battle(Battle battle, IPieceModel attacker, IPieceModel defender)
        {
            Assert.IsNotNull(attacker);
            Assert.IsNotNull(defender);
            Assert.AreNotSame(attacker, defender);

            attacker.Attack(defender);

            return Response.Ok;
        }

        private Response NotImplemented(IRequest req, string text = "")
        {
            return Failed(req, $"Not Implemented {text}");
        }

        private Response Failed(IRequest req, string text = "")
        {
            Error(text);
            req.Player.RequestFailed(req);
            return new Response(EResponse.Fail, EError.Error, text);
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
