// user can play cards without worrying about mana
//#define IGNORE_MANA

// DI fails this inspection test
// ReSharper disable UnassignedGetOnlyAutoProperty

using System.ComponentModel.Design;

namespace App.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    using Dekuple;
    using Common;
    using Common.Message;

    /// <summary>
    /// The ArbiterModel enforces the rules. The BoardModel provides state information and queries.
    /// </summary>
    public class ArbiterModel
        : RespondingModelBase
        , IArbiterModel
    {
        public EColor Color => EColor.Neutral;
        public IReadOnlyReactiveProperty<EGameState> GameState => _gameState;
        public IReactiveProperty<int> TurnNumber => _turnNumber;
        public IReadOnlyReactiveProperty<IPlayerModel> CurrentPlayer => _currentPlayer;
        public IReadOnlyReactiveProperty<RequestResponse> LastResponse => _lastResponse;
        public IReadOnlyReactiveProperty<string> Log => _log;

        [Inject] public IBoardModel Board { get; set; }

        private List<PlayerEntry> _players;
        private readonly IntReactiveProperty _turnNumber = new IntReactiveProperty();
        private readonly IntReactiveProperty _currentPlayerIndex = new IntReactiveProperty(0);
        private readonly ReactiveProperty<IPlayerModel> _currentPlayer = new ReactiveProperty<IPlayerModel>();
        private readonly ReactiveProperty<EGameState> _gameState = new ReactiveProperty<EGameState>(EGameState.PlayTurn);
        private readonly ReactiveProperty<RequestResponse> _lastResponse = new ReactiveProperty<RequestResponse>();
        private readonly ReactiveProperty<string> _log = new ReactiveProperty<string>();

        public ArbiterModel()
            : base(null)
        {
            LogPrefix = "Arbiter";
            //Verbosity = 100;
        }

        public void PrepareGame(IPlayerModel w, IPlayerModel b)
        {
            _players = new List<PlayerEntry>()
            {
                new PlayerEntry(w),
                new PlayerEntry(b),
            };
            
            _currentPlayerIndex.Subscribe(n => _currentPlayer.Value = _players[n].Player);
            _currentPlayerIndex.Value = 0;
        }

        public void StartGame()
        {
            _gameState.Value = EGameState.Start;
            Board.StartGame();
            foreach (var p in _players)
                p.Player.StartGame();

            // TODO: start properly
            _currentPlayerIndex.Value = 0;
            _gameState.Value = EGameState.PlayTurn;
        }

        public void EndGame()
        {
            _gameState.Value = EGameState.Completed;
            Info("EndGame");
        }

        public IResponse Arbitrate(IGameRequest request)
        {
            Assert.IsNotNull(request);
            var response = ProcessRequest(request);
            Verbose(10, $"Arbitrate: {request} {response}");
            var player = request.Owner as IPlayerModel;
            if (!response.Failed)
            {
            }

            player?.Result(request, response);
            
            _lastResponse.Value = new RequestResponse { Request = request, Response = response };
            return response;
        }

        private bool IsInCheck(IPlayerModel player)
        {
            if (player == null)
                return false;

            var color = player.Color;
            var king = Board.FirstOrDefault(color, EPieceType.King);
            if (king == null)
                return false;
            
            return IsInCheck(color, king.Coord.Value);
        }
            
        private bool IsInCheck(EColor color, Coord coord)
        {
            var checking = Board.TestForCheck(color, coord).ToArray();
            foreach (var ch in checking)
                Info($"Piece {ch} puts {color} in check.");

            return checking.Length > 0;
        }

        /// <summary>
        /// TODO: this is going to be fun when Instants are added
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool IsInCheckMate(IPlayerModel player)
        {
            return false;
        }

        private IResponse ProcessRequest(IGameRequest request)
        {
            Verbose(20, $"{request} in {GameState} #{_turnNumber.Value}");
            switch (GameState.Value)
            {
                //case EGameState.None:
                //    return Response.Ok;
                //case EGameState.Start:
                //    return TryStartGame(request as StartGame);
                //case EGameState.Mulligan:
                //    return TryRejectCards(request as RejectCards);
                //case EGameState.PlaceKing:
                //    return TryPlaceKing(request as PlacePiece);
                //case EGameState.TurnEnd:
                //    return TryTurnEnd(request as TurnEnd);
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
                        case EActionType.GiveItem:
                            return TryGiveItem(request as GiveItem);
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

        private Response TryStartGame(StartGame startGame)
        {
            var entry = GetEntry(startGame.Player);
            if (entry.Started)
            {
                Warn($"{entry.Player} has already startd game");
                return Response.Fail;
            }
            entry.Started = true;
            if (_players.All(p => p.Started))
                _gameState.Value = EGameState.Mulligan;

            return Response.Ok;
        }

        public void EndTurn()
        {
            Verbose(10, $"End turn #{_turnNumber.Value} for {CurrentPlayer.Value}");

            CurrentPlayer.Value.EndTurn();
            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            _turnNumber.Value++;
            CurrentPlayer.Value.StartTurn();

            foreach (var entry in _players)
                entry.NewTurn();

            Board.NewTurn();

            _gameState.Value = EGameState.PlayTurn;
        }

        private void Endame()
        {
            Info("EndGame");
            _gameState.Value = EGameState.Completed;
        }

        private Response TryRejectCards(RejectCards rejections)
        {
            Assert.IsNotNull(rejections);

            // TODO: deal with mulligan of type RejectCards
            var entry = GetEntry(rejections.Player);
            if (entry.RejectedCards)
            {
                Error($"Player {rejections.Player} cannot reject twice");
                return Response.Fail;
            }
            // TODO: give player different cards in response
            entry.RejectedCards = true;
            if (_players.All(a => a.RejectedCards))
                _gameState.Value = EGameState.PlaceKing;

            return Response.Ok;
        }

        private IResponse TryMovePiece(MovePiece move)
        {
            var player = move.Player;
            var piece = move.Piece;
            if (GameState.Value != EGameState.PlayTurn)
                return Failed(move, $"Currently in {GameState}, {player} cannot move {piece}.");

            if (piece.MovedThisTurn)
                return Failed(move, $"{piece} can only move once per turn.");

            if (CurrentPlayer.Value != player)
                return Failed(move, $"Not {player}'s turn.");

            if (player.Mana.Value < 1)
                return Failed(move, "Requires 1 mana to move a piece.");

            if (GetEntry(move.Player).MovedPiece)
                return Failed(move, $"{player} has already moved a piece this turn.");

            var resp = Board.TryMovePiece(move);
            if (resp.Success)
            {
                piece.MovedThisTurn = true;
                player.ChangeMana(-1);
            }

            return resp;
        }

        private IResponse<IPieceModel> TryPlacePiece(PlacePiece act)
        {
            var entry = GetEntry(act.Player);
            var card = act.Card;
            
            // Have to start with a King.
            var isKing = card.PieceType == EPieceType.King;
            if (!entry.PlacedKing && !isKing)
                return Response<IPieceModel>.FailWith("Must place king first.");
            
            if (isKing && IsInCheck(card.Color, act.Coord))
                return Response<IPieceModel>.FailWith("Can't place king in check.");

            // Can only have one Queen.
            var owner = act.Owner as IPlayerModel;
            if (act.Card.PieceType == EPieceType.Queen)
            {
                var otherQueen = Board.Pieces.Any(p => p.SameOwner(owner) && p.PieceType == EPieceType.Queen);
                if (otherQueen)
                    return Response<IPieceModel>.FailWith("Can have up to one Queen on Board at a time.");

                var nearKing = Board.GetAdjacent(
                    act.Coord, 1).Interrupts.Any(
                    p => p.SameOwner(owner) && p.PieceType == EPieceType.King);
                if (!nearKing)
                    return Response<IPieceModel>.FailWith("Queens must be placed next to a King.");
            }
            
            // Check mana cost.
            var playerMana = act.Player.Mana;
            var manaCost = act.Card.ManaCost;
#if !IGNORE_MANA
            if (playerMana.Value - manaCost.Value < 0)
                return Response<IPieceModel>.FailWith("Not enough mana.");
#endif

            // Pawns can only be placed next to a friendly.
            if (card.PieceType == EPieceType.Peon
                && !Board.GetAdjacent(act.Coord, 1).Interrupts.Any(other => other.SameOwner(card)))
            {
                return Response<IPieceModel>.FailWith("Peons must be placed next to friendly pieces.");
            }

            // Let the board make the final decision based on its own state.
            var resp = Board.TryPlacePiece(act);
            if (resp.Failed)
                return resp;
            
            // We can place the card!
            playerMana.Value -= manaCost.Value;
            if (isKing)
                entry.PlacedKing = true;

            return resp;
        }

        private IEnumerable<IPieceModel> OtherPieces(ICardModel card)
            => Board.Pieces.Where(p => p.Color != card.Color);

        private Response TryResign(Resign resign)
        {
            _gameState.Value = EGameState.Completed;
            return Response.Ok;
        }

        private IResponse TryBattle(Battle battle)
        {
            Assert.IsNotNull(battle);
            var attacker = battle.Attacker;
            var defender = battle.Defender;
            Assert.IsNotNull(defender);
            Assert.IsNotNull(attacker);

            if (attacker.SameOwner(defender))
                return Failed(battle, $"{attacker} can't battle own piece.");

            if (attacker.AttackedThisTurn)
                return Failed(battle, $"{attacker} can only attack once per turn.");

            var moves = Board.GetAttacks(attacker.Coord.Value, attacker.PieceType);
            if (!moves.Interrupts.Contains(defender))
                return Failed(battle, $"{attacker} can not reach {defender.Coord.Value}.");

            var resp = attacker.Attack(defender);
            if (!resp.Success)
                return resp;
            
            if (attacker.Dead.Value)
                _log.Value = $"{attacker} died.";
                    
            if (defender.Dead.Value)
                _log.Value = $"{defender} died.";
                
            attacker.AttackedThisTurn = true;

            return resp;
        }

        private IResponse TryTurnEnd(TurnEnd turnEnd)
        {
            Assert.IsNotNull(turnEnd);
            var player = turnEnd.Owner as IPlayerModel;
            if (turnEnd.Player != CurrentPlayer.Value)
            {
                Warn($"Arbitrate: It's not {turnEnd.Player}'s turn to end.");
                return new Response(turnEnd, EResponse.Fail, EError.Error, "Not {turnEnd.Players}'s turn to end.");
            }
            
            if (IsInCheck(player))
            {
                var b = IsInCheck(player);
                Warn($"Arbitrate: {turnEnd} leaves {player}'s king in check, failing.");
                return new Response(turnEnd, EResponse.Fail, EError.Error, "Can't leave King in Check");
            }
            
            EndTurn();
            return Response.Ok;
        }

        private IResponse TryCastSpell(CastSpell castSpell)
        {
            Assert.IsNotNull(castSpell);
            return NotImplemented(castSpell, $"{castSpell}");
        }

        private IResponse TryGiveItem(GiveItem giveItem)
        {
            return NotImplemented(giveItem);
        }

        private PlayerEntry GetEntry(IPlayerModel player)
        {
            Assert.IsNotNull(player);
            Assert.IsNotNull(_players);
            Assert.IsTrue(_players.Count == 2);
            Assert.IsTrue(_players.Count(p => p.Player == player) == 1);
            return _players.First(p => p.Player == player);
        }

        class PlayerEntry
        {
            public bool Started;
            public bool RejectedCards;
            public bool MovedPiece;
            public bool PlacedKing;
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
    }
}
