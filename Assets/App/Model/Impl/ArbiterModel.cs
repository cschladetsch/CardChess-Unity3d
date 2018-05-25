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
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;
        public IPlayerModel WhitePlayer { get; private set; }
        public IPlayerModel BlackPlayer { get; private set; }
        public IPlayerModel CurrentPlayer => _players[_currentPlayer];
        public IPlayerModel OtherPlayer => _players[(_currentPlayer + 1) % 2];

        #endregion

        #region Public Methods

        public ArbiterModel()
        {
        }

        public void NewGame(IPlayerModel w, IPlayerModel b)
        {
            WhitePlayer = w;
            BlackPlayer = b;
            _players = new[] { w, b };
            _accepted = new[] {false, false};
            _currentPlayer = 0;

            Construct(this);
            Board.NewGame();
            foreach (var player in _players)
                player.NewGame();

            GameState = EGameState.Mulligan;
        }

        public void PlayerMulligan(IPlayerModel player, IEnumerable<ICardModel> cards)
        {
            NotImplemented();
        }

        public void PlayerAcceptCards(IPlayerModel player)
        {
            Info($"Player {player} accepted cards");
            Assert.AreEqual(GameState, EGameState.Mulligan);
            _accepted[IndexOf(player)] = true;
            if (!_accepted.All(b => b))
                return;
            GameState = EGameState.PlaceKing;
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
                        GameState = EGameState.TurnStart;
                    }
                    return resp;
                }
                case EGameState.TurnPlay:
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

            if (GameState != EGameState.TurnPlay)
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

        public Response Arbitrate(IRequest request)
        {
            Verbose(10, $"Handling {request} when in state {GameState}");
            switch (GameState)
            {
                case EGameState.None:
                    return Response.Ok;
                case EGameState.Start:
                    return Response.Ok;
                case EGameState.Mulligan:
                    _accepted[IndexOf(request.Player)] = true;
                    if (_accepted.All(a => a))
                        GameState = EGameState.PlaceKing;
                    return Response.Ok;
                case EGameState.PlaceKing:
                    return RequestPlayCard(request as PlayCard);
                case EGameState.TurnStart:
                    return Response.Ok;
                case EGameState.TurnPlay:
                    switch (request.Action)
                    {
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
                    return Response.Ok;
                case EGameState.Battle:
                    break;
                case EGameState.TurnEnd:
                    _currentPlayer = (_currentPlayer + 1) % _players.Length;
                    return Response.Ok;
                case EGameState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return NotImplemented($"{request}");
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

        private Response Failed(string text)
        {
            Warn(text);
            return Response.Fail;
        }

        private int IndexOf(IPlayerModel player)
        {
            for (int n = 0; n < _players.Length; ++n)
                if (_players[n] == player)
                    return n;
            throw new System.Exception();
        }

        #endregion

        #region Private Fields

        bool[] _accepted = new bool[2] { false, false};
        private IPlayerModel[] _players;
        private int _currentPlayer;

        #endregion
    }
}
