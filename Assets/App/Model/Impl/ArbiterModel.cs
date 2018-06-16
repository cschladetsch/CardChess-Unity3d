// user can play cards without worrying about mana
#define IGNORE_MANA

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
        public EColor Color => EColor.Neutral;
        public IReadOnlyReactiveProperty<EGameState> GameState => _gameState;
        public IReactiveProperty<int> TurnNumber => _turnNumber;
        public IReadOnlyReactiveProperty<IPlayerModel> CurrentPlayer => _currentPlayer;

        [Inject]
        public IBoardModel Board { get; set; }

        public ArbiterModel()
            : base(null)
        {
            LogPrefix = "Arbiter";
        }

        public override void PrepareModels()
        {
            base.PrepareModels();
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

        public override void StartGame()
        {
            _gameState.Value = EGameState.Start;
            Board.StartGame();
            foreach (var p in _players)
                p.Player.StartGame();

            // TODO: start properly
            _currentPlayerIndex.Value = 0;
            _gameState.Value = EGameState.PlayTurn;
        }

        public override void EndGame()
        {
            _gameState.Value = EGameState.Completed;
            Info("EndGame");
        }

        public IResponse Arbitrate(IRequest request)
        {
            Assert.IsNotNull(request);
            //Info($"Arbitrate: {request} Id={request.Id}");
            var response = ProcessRequest(request);
            request.Player?.Result(request, response);
            return response;
        }

        private IResponse ProcessRequest(IRequest request)
        {
            Info($"{request} tried in {GameState} #{_turnNumber.Value}");
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

        private void StartFirstTurn()
        {
            _currentPlayerIndex.Value = 0;
            CurrentPlayer.Value.StartTurn();
            _gameState.Value = EGameState.PlayTurn;
        }

        public void EndTurn()
        {
            //Info($"End turn #{_turnNumber.Value} for {CurrentPlayer.Value}");

            CurrentPlayer.Value.EndTurn();
            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            _turnNumber.Value++;
            CurrentPlayer.Value.StartTurn();
            foreach (var entry in _players)
                entry.NewTurn();

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
                return Failed(move, $"Currently in {GameState}, {player} cannot move {piece}");

            if (CurrentPlayer.Value != player)
                return Failed(move, $"Not {player}'s turn");

            if (player.Mana.Value < 1)
                return Failed(move, "Requires 1 mana to move a piece");

            if (GetEntry(move.Player).MovedPiece)
                return Failed(move, $"{player} has already moved a piece this turn");

            var resp = Board.TryMovePiece(move);
            if (resp.Success)
            {
                player.ChangeMana(-1);
            }

            return resp;
        }

        private IResponse<IPieceModel> TryPlacePiece(PlacePiece act)
        {
            var entry = GetEntry(act.Player);
            var isKing = act.Card.PieceType == EPieceType.King;
            if (!entry.PlacedKing && !isKing)
                return new Response<IPieceModel>(null, EResponse.Fail, EError.Error, "Must place king first");

            var playerMana = act.Player.Mana;
            var manaCost = act.Card.ManaCost;
#if !IGNORE_MANA
            if (playerMana.Value - manaCost.Value < 0)
            {
                Warn($"{act.Player} deoesn't have mana to play {act.Card}");
                return new Response<IPieceModel>(null, EResponse.Fail, EError.NotEnoughMana, $"Attempted {act}");
            }
#endif

            var resp = Board.TryPlacePiece(act);
            if (resp.Failed)
            {
                Warn($"{act} failed");
                return resp;
            }
            playerMana.Value -= manaCost.Value;
            if (isKing)
                entry.PlacedKing = true;

            return resp;
        }

        private Response TryResign(Resign resign)
        {
            Warn($"{resign}");
            _gameState.Value = EGameState.Completed;
            return Response.Ok;
        }

        private IResponse TryBattle(Battle battle)
        {
            Assert.IsNotNull(battle);
            Assert.IsNotNull(battle.Attacker);
            Assert.IsNotNull(battle.Defender);

            if (battle.Attacker.SameOwner(battle.Defender))
                return Failed(battle, "Can't battle own piece");

            return battle.Attacker.Attack(battle.Defender);
        }

        private IResponse TryTurnEnd(TurnEnd turnEnd)
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

        private List<PlayerEntry> _players;
        private readonly IntReactiveProperty _turnNumber = new IntReactiveProperty();
        private readonly IntReactiveProperty _currentPlayerIndex = new IntReactiveProperty(0);
        private readonly ReactiveProperty<IPlayerModel> _currentPlayer = new ReactiveProperty<IPlayerModel>();
        private readonly ReactiveProperty<EGameState> _gameState = new ReactiveProperty<EGameState>(EGameState.PlayTurn);
    }
}
