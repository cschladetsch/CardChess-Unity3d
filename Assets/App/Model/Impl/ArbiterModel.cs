using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

// DI fails this inspection test
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace App.Model
{
    using Registry;
    using Common.Message;
    using Common;

    public class ArbiterModel
        : RespondingModelBase
        , IArbiterModel
    {
        #region Public Properties
        public EColor Color => EColor.Neutral;
        public IReadOnlyReactiveProperty<EGameState> GameState => _gameState;
        public IReadOnlyReactiveProperty<IPlayerModel> CurrentPlayer => _currentPlayer;
        [Inject] public IBoardModel Board { get; set; }
        #endregion

        #region Public Methods
        public ArbiterModel()
            : base(null)
        {
            LogPrefix = "Arbiter";
        }

        public void NewGame(IPlayerModel w, IPlayerModel b)
        {
            _players = new List<PlayerEntry>()
            {
                new PlayerEntry(w),
                new PlayerEntry(b),
            };
            _currentPlayerIndex.Subscribe(n => _currentPlayer.Value = _players[n].Player);
            _currentPlayerIndex.Value = 0;

            Board.NewGame();
            foreach (var entry in _players)
                entry.Player.NewGame();
        }

        public Response Arbitrate(IRequest request)
        {
            Info($"{request} in {GameState} #{_turnNumber}");
            switch (GameState.Value)
            {
                case EGameState.None:
                    return Response.Ok;
                case EGameState.Start:
                    return Response.Ok;
                case EGameState.Mulligan:
                    return TryAcceptCards(request);
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

        #endregion

        #region Private Methods
        private void StartFirstTurn()
        {
            _currentPlayerIndex.Value = 0;
            CurrentPlayer.Value.StartTurn();
            _gameState.Value = EGameState.PlayTurn;
        }

        private void EndTurn()
        {
            CurrentPlayer.Value.EndTurn();
            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            _turnNumber++;
            CurrentPlayer.Value.StartTurn();
            foreach (var entry in _players)
                entry.NewTurn();

            Verbose(10, $"Next turn #{_turnNumber} for {CurrentPlayer}");
            _gameState.Value = EGameState.PlayTurn;
        }

        private void Endame()
        {
            Info("EndGame");
            _gameState.Value = EGameState.Completed;
        }

        private Response TryAcceptCards(IRequest request)
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
                _gameState.Value = EGameState.PlaceKing;

            return Response.Ok;
        }

        private Response TryPlaceKing(PlacePiece act)
        {
            Assert.IsNotNull(act);
            Assert.IsNotNull(act.Player);
            Assert.IsNotNull(act.Card);
            Assert.AreEqual(EGameState.PlaceKing, GameState.Value);
            Assert.AreEqual(act.Card.PieceType, EPieceType.King);

            if (Board.GetAdjacent(act.Coord, 2).Any(k => k.Type == EPieceType.King))
            {
                return Failed(act, $"{act.Player} must place king further away from enemy king", EError.TooClose);
            }

            var resp = TryPlacePiece(act);
            if (resp.Type != EResponse.Ok)
                return Failed(act, $"Couldn't place {act.Player}'s king at {act.Coord}");

            act.Player.KingPiece = resp.Payload;
            if (Board.NumPieces(EPieceType.King) == 2)
            {
                StartFirstTurn();
            }

            return Response.Ok;
        }

        private Response TryMovePiece(MovePiece move)
        {
            var player = move.Player;
            var piece = move.Piece;
            if (GameState.Value != EGameState.PlayTurn)
                return Failed(move, $"Currently in {GameState}, {player} cannot move {piece}");

            if (CurrentPlayer.Value != player)
                return Failed(move, $"Not {player}'s turn");

            if (GetEntry(move.Player).MovedPiece)
                return Failed(move, $"{player} has already moved a piece this turn");

            var resp = Board.TryMovePiece(move);
            if (resp.Success)
                EndTurn();
            return resp;
        }

        private Response<IPieceModel> TryPlacePiece(PlacePiece act)
        {
            var playerMana = act.Player.Mana;
            var manaCost = act.Card.ManaCost;
            if (playerMana.Value - manaCost.Value < 0)
            {
                Warn($"{act.Player} deoesn't have mana to play {act.Card}");
                return new Response<IPieceModel>(null, EResponse.Fail, EError.NotEnoughMana, $"Attempted {act}");
            }

            var resp = Board.TryPlacePiece(act);
            if (resp.Failed)
                return resp;

            playerMana.Value -= manaCost.Value;
            EndTurn();
            return resp;
        }

        private Response TryResign(Resign resign)
        {
            return NotImplemented(resign);
        }

        private Response TryBattle(Battle battle)
        {
            Assert.IsNotNull(battle);
            Assert.IsNotNull(battle.Attacker);
            Assert.IsNotNull(battle.Defender);

            return battle.Attacker.Attack(battle.Defender);
        }

        private Response TryTurnEnd(TurnEnd turnEnd)
        {
            Assert.IsNotNull(turnEnd);
            if (turnEnd.Player != CurrentPlayer.Value)
            {
                Warn($"It's not {turnEnd.Player}'s turn to end");
                return Response.Ok;
            }

            EndTurn();

            return Response.Ok;
        }

        private Response TryCastSpell(CastSpell castSpell)
        {
            Assert.IsNotNull(castSpell);
            return NotImplemented(castSpell, $"{castSpell}");
        }

        private Response TryGiveItem(GiveItem giveItem)
        {
            return NotImplemented(giveItem);
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
        private int _turnNumber;
        private readonly IntReactiveProperty _currentPlayerIndex = new IntReactiveProperty(0);
        private readonly ReactiveProperty<IPlayerModel> _currentPlayer = new ReactiveProperty<IPlayerModel>();
        private readonly ReactiveProperty<EGameState> _gameState = new ReactiveProperty<EGameState>(EGameState.Mulligan);
        #endregion
    }
}
