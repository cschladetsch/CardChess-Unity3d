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
			_currentPlayer = 0;

			Construct(this);
			Board.NewGame();
			foreach (var p in _players)
				p.NewGame();

			GameState = EGameState.Ready;
		}

		public void Endame()
		{
			GameState = EGameState.Completed;
		}

		public Response RequestPlayCard(IPlayerModel player, ICardModel card)
		{
			return Response.Fail;
		}

		public Response RequestPlayCard(IPlayerModel player, ICardModel card, Coord coord)
		{
            // check for empty target square
			var piece = Board.At(coord);
			if (piece != null)
			{
				// TODO: check for mounting
				Warn($"There is already a card at {coord}");
				return Response.Fail;
			}

            // check that player can play a card in current game state
			var canAttempt = GameState == EGameState.TurnPlay && CurrentPlayer == player;
            if (!canAttempt)
            {
                Warn($"Player {player} cannot play {card} now");
                return Response.Fail;
            }

            // make the piece
			piece = Registry.New<IPieceModel>(player, card);
            if (piece == null)
            {
                Error($"Player {player} couldn't make a piece using {card}");
                return Response.Fail;
            }

            // check that the piece can actually move there
            var possibleMoves = Board.GetMovements(coord);
			if (!possibleMoves.Contains(coord))
			{
				Warn($"Player {player} cannot move {card} to {coord}");
				return Response.Fail;
			}

			if (piece.Type == EPieceType.King)
			{
				var other = Board.GetPieces(EPieceType.King).FirstOrDefault();
				if (other.Coord.MaxOrthoDistance(coord) < 2)
				{
					Warn($"Player cannot play King so close to opposing king");
					return Response.Fail;
				}
			}
			bool placed = Board.PlacePiece(piece, coord);
			return placed ? Response.Ok : Response.Fail;
		}

		public Response RequestMovePiece(IPlayerModel player, IPieceModel wk, Coord coord)
		{
			return Response.Fail;
		}

		#endregion

		#region Private Fields
		private IPlayerModel[] _players;
        private int _currentPlayer;

        #endregion
    }
}
