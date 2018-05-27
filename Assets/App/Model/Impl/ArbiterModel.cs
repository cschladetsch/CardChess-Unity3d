using System;
using System.Collections.Generic;
using System.Linq;

// DI fails this inspection test
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace App.Model
{
    using Registry;
    using Common.Message;
    using Common;

    public class ArbiterModel
        : RespondingModelBase
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
            LogPrefix = "Arbiter";
            Verbosity = 100;
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
            return Board.TryPlacePiece(act);
        }

        public Response TryPlaceKing(PlacePiece act)
        {
            Assert.IsNotNull(act);
            Assert.IsNotNull(act.Player);
            Assert.IsNotNull(act.Card);
            Assert.AreEqual(EGameState.PlaceKing, GameState);
            Assert.AreEqual(act.Card.PieceType, EPieceType.King);

            if (Board.GetAdjacent(act.Coord, 2).Any(k => k.Type == EPieceType.King))
            {
                return Failed(act, $"{act.Player} must place king further away from enemy king", EError.TooClose);
            }

            var resp = TryPlacePiece(act);
            if (resp.Type != EResponse.Ok)
                return Failed(act, $"Couldn't place {act.Player}'s king at {act.Coord}");

            act.Player.KingPiece = Board.GetPieces(EPieceType.King).First(k => k.Owner == act.Player);
            if (Board.NumPieces(EPieceType.King) == 2)
            {
                StartFirstTurn();
            }

            return Response.Ok;
        }

        private void StartFirstTurn()
        {
            _currentPlayer = 0;
            CurrentPlayer.StartTurn();
            GameState = EGameState.PlayTurn;
        }

        private Response TryMovePiece(MovePiece move)
        {
            var player = move.Player;
            var piece = move.Piece;
            if (GameState != EGameState.PlayTurn)
                return Failed(move, $"Currently in {GameState}, {player} cannot move {piece}");

            if (CurrentPlayer != player)
                return Failed(move, $"Not {player}'s turn");

            if (GetEntry(move.Player).MovedPiece)
                return Failed(move, $"{player} has already moved a piece this turn");

            return Board.TryMovePiece(move);
        }

        private Response TryRejectCards(IRequest request)
        {
            // TODO: deal with mulligan of type RejectCards
            var entry = GetEntry(request.Player);
            if (entry.AcceptedCards)
            {
                Error($"Player {request.Player} cannot Accept twice");
                return Response.Fail;
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
                        //case EActionType.Pass:
                        //    return TryTurnPass(request as TurnPass());
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
            Assert.IsNotNull(battle);
            Assert.IsNotNull(battle.Attacker);
            Assert.IsNotNull(battle.Defender);

            return battle.Attacker.Attack(battle.Defender);
        }

        Response TryTurnEnd(TurnEnd turnEnd)
        {
            Assert.IsNotNull(turnEnd);
            if (turnEnd.Player != CurrentPlayer)
            {
                Warn($"It's not {turnEnd.Player}'s turn to end");
                return Response.Ok;
            }

            EndTurn();

            return Response.Ok;
        }

        private void EndTurn()
        {
            CurrentPlayer.EndTurn();
            _currentPlayer = (_currentPlayer + 1) % _players.Count;
            _turnNumber++;
            CurrentPlayer.StartTurn();
            foreach (var entry in _players)
                entry.NewTurn();

            Verbose(10, $"Next turn #{_turnNumber} for {CurrentPlayer}");
            GameState = EGameState.PlayTurn;
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

        private PlayerEntry GetEntry(IPlayerModel player)
        {
            Assert.IsNotNull(player);
            Assert.IsTrue(_players.Count(p => p.Player == player) == 1);
            return _players.First(p => p.Player == player);
        }

        #endregion

        #region Private Fields

        class PlayerEntry
        {
            public bool AcceptedCards;
            public bool MovedPiece;
            public readonly IPlayerModel Player;

            public PlayerEntry(IPlayerModel player)
            {
                Player = player;
            }

            public void NewTurn()
            {
                MovedPiece = false;
            }
        }

        private List<PlayerEntry> _players;
        private int _currentPlayer;
        private int _turnNumber;

        #endregion
    }
}
