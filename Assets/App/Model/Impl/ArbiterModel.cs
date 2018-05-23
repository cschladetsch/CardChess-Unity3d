using System;
using System.Collections.Generic;
using System.Linq;
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

        public Response RequestPlayCard(IPlayerModel player, ICardModel card)
        {
            return Failed("No spells yet");
        }

        private Response TryPlayCard(IPlayerModel player, ICardModel card, Coord coord)
        {
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

        public Response RequestPlayCard(IPlayerModel player, ICardModel card, Coord coord)
        {
            Assert.IsNotNull(player);
            Assert.IsNotNull(card);
            Assert.IsNotNull(coord);
            Assert.IsTrue(Board.IsValidCoord(coord));

            switch (GameState)
            {
                case EGameState.PlaceKing:
                {
                    var resp = TryPlayCard(player, card, coord);
                    if (resp.Type == EResponse.Ok && Board.NumPieces(EPieceType.King) == 2)
                    {
                        _currentPlayer = 0;
                        GameState = EGameState.TurnStart;
                    }
                    return resp;
                }
                case EGameState.TurnPlay:
                    return TryPlayCard(player, card, coord);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Response RequestMovePiece(IPlayerModel player, IPieceModel piece, Coord coord)
        {
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
            if (!Board.GetMovements(coord).Contains(coord))
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
