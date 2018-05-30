using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace App.Model
{
    using Common;
    using Common.Message;
    using Registry;

    /// <summary>
    /// The main playing board Can be of arbitrary dimention.
    /// Contents are stored as row-major. the bottom left corner for white is at contents[0][0]
    /// the topright corner for white is at contents[Height - 1][Width - 1]
    /// Both Black and White use the same coordinate system.
    /// </summary>
    public class BoardModel
        : RespondingModelBase
        , IBoardModel
    {
        #region Public Properties
        public int Width { get; }
        public int Height { get; }
        [Inject] public IArbiterModel Arbiter { get; set; }
        public IEnumerable<IPieceModel> Pieces => _pieces.Where(p => p != null);
        #endregion

        #region Public Methods

        public BoardModel(int width, int height)
            : base(null)
        {
            Width = width;
            Height = height;
        }

        public void NewGame()
        {
            ClearBoard();
            ConstructBoard();
        }

        public IEnumerable<IPieceModel> PiecesOfType(EPieceType type)
        {
            return Pieces.Where(p => p.Type == type);
        }

        public int NumPieces(EPieceType type)
        {
            return PiecesOfType(type).Count();
        }

        public Response Remove(IPieceModel pieceModel)
        {
            Assert.IsNotNull(pieceModel);
            Verbose(5, $"Removing {pieceModel} from board");
            return Set(pieceModel.Coord.Value, null);
        }

        public IPieceModel RemovePiece(Coord coord)
        {
            if (!IsValid(coord))
            {
                Warn($"Attempt to remove from invalid {coord}");
                return null;
            }
            var current = At(coord);
            Set(coord, null);
            return current;
        }

        public bool IsValid(Coord coord)
        {
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }

        public Response TryMovePiece(MovePiece move)
        {
            var coord = move.Coord;
            var piece = move.Piece;

            return GetMovements(piece.Coord.Value).Any(c => At(c) == null)
                ? MovePieceTo(coord, piece)
                : Failed(move, $"{move.Player} cannot move {piece} to {coord}: illegal movmenet");
        }

        private Response MovePieceTo(Coord coord, IPieceModel piece)
        {
            var old = piece.Coord.Value;
            var resp = Set(coord, piece);
            return resp.Success ? Set(old, null) : resp;
        }

        public IEnumerable<IPieceModel> GetAdjacent(Coord coord, int dist = 1)
        {
            var items = new List<IPieceModel>();
            for (var y = -dist; y <= dist; ++y)
            {
                for (var x = -dist; x <= dist; ++x)
                {
                    if (x == 0 && y == 0)
                        continue;

                    var piece = At(coord);
                    if (piece == null)
                        continue;

                    items.Add(piece);
                }
            }
            return items;
        }

        public IEnumerable<IPieceModel> AttackedCards(Coord coord)
        {
            var piece = At(coord);
            if (piece == null)
                yield break;
            foreach (var c in GetPossibleMovements(piece))
            {
                var attacked = At(c);
                if (attacked != null)
                    yield return attacked;
            }
        }

        public IEnumerable<IPieceModel> DefendededCards(IPieceModel defender, Coord cood)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Defenders(Coord cood)
        {
            throw new NotImplementedException();
        }

        public IPieceModel GetContents(Coord coord)
        {
            return !IsValidCoord(coord) ? null : At(coord);
        }

        public IPieceModel At(Coord coord)
        {
            Assert.IsTrue(IsValid(coord));
            return At(coord.x, coord.y);
        }

        public IPieceModel At(int x, int y)
        {
            if (x < 0 || y < 0)
                return null;
            if (x >= Width || y >= Height)
                return null;
            return _pieces[y * Width + x];
        }

        public bool IsValidCoord(Coord coord)
        {
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }

        public Response<IPieceModel> TryPlacePiece(PlacePiece placePiece)
        {
            Assert.IsNotNull(placePiece);
            var coord = placePiece.Coord;
            Assert.IsTrue(IsValid(coord));

            if (At(coord) != null)
                return new Response<IPieceModel>(null, EResponse.Fail, EError.InvalidTarget, $"Already {At(coord)}, cannot {placePiece}");

            var piece = Registry.New<IPieceModel>(placePiece.Player, placePiece.Card);
            Set(coord, piece);

            Verbose(20, $"{placePiece}");
            return new Response<IPieceModel>(piece);
        }

        public Response Remove(PieceModel piece)
        {
            Assert.IsNotNull(piece);
            return Set(piece.Coord.Value, null);
        }

        public IEnumerable<Coord> GetMovements(Coord coord)
        {
            var piece = At(coord);
            return piece == null ? null : GetMovements(piece, coord);
        }

        public IEnumerable<Coord> GetMovements(IPieceModel piece, Coord coord)
        {
            switch (piece.Type)
            {
                case EPieceType.King:
                    foreach (var c in Nearby(coord, 1))
                        yield return c;
                    break;
                case EPieceType.Peon:
                    {
                        var delta = piece.Color == EColor.White ? 1 : -1;
                        yield return new Coord(coord.x, coord.y + delta);
                    }
                    break;
                case EPieceType.Archer:
                    foreach (var c in Diagonals(coord, Math.Max(Width, Height)))
                        yield return c;
                    break;
            }
        }

        public string Print()
        {
            return Print(coord =>
            {
                var piece = At(coord);
                var rep = CardToRep(piece.Card);
                var black = piece.Owner.Value.Color != EColor.White;
                if (black)
                    rep = rep.ToLower();
                return rep;
            });
        }

        public string Print(Func<Coord, string> fun)
        {
            var sb = new StringBuilder();
            for (int y = Height - 1; y >= 0; --y)
            {
                // vertical axis
                sb.Append($" {y}:");

                // horizontal cards
                for (int x = 0; x < Width; ++x)
                {
                    var coord = new Coord(x, y);
                    var rep = "  ";
                    if (At(coord) != null)
                        rep = $"{fun(coord)}";
                    sb.Append(rep);
                }
                sb.AppendLine();
                if (y == 0)
                {
                    // write the bottom axis
                    sb.Append("   ");
                    for (int x = 0; x < Width; ++x)
                        sb.Append($"{x} ");
                }
            }
            return sb.ToString();
        }

        public string CardToRep(ICardModel model)
        {
            if (model == null) return "  ";
            var ch = $"{model.PieceType.ToString()[0]} ";
            return ch;
        }

        #endregion

        #region Private Methods

        private void ConstructBoard()
        {
            _pieces = new ReactiveCollection<IPieceModel>();
            for (var n = 0; n < Width*Height; ++n)
                _pieces.Add(null);  // TODO: use empty PieceModels
        }

        private Response Set(Coord coord, IPieceModel piece)
        {
            Assert.IsTrue(IsValid(coord));
            _pieces[coord.y * Width + coord.x] = piece;
            if (piece != null)
                piece.Coord.Value = coord;
            return Response.Ok;
        }

        private void ClearBoard()
        {
            if (_pieces == null)
                return;

            foreach (var card in _pieces)
            {
                if (card == null)
                    continue;
                RemovePiece(card.Coord.Value);
                card.Destroy();
            }
        }

        private IEnumerable<Coord> Nearby(Coord orig, int dist)
        {
            var y = Math.Max(orig.y - dist, 0);
            for (; y <= orig.y + dist; ++y)
            {
                for (var x = Math.Max(orig.x - dist, 0); x <= orig.x + dist; ++x)
                {
                    var coord = new Coord(x, y);
                    if (!IsValid(coord))
                        break;
                    if (!Equals(coord, orig))
                        yield return coord;
                }
            }
        }

        private IEnumerable<Coord> Diagonals(Coord orig, int dist)
        {
            for (int dx = -1; dx < 2; dx++)
            {
                if (dx == 0) continue;
                for (int dy = -1; dy < 2; dy++)
                {
                    if (dy == 0) continue;
                    foreach (var c in TestCoords(orig, dx, dy, dist))
                        yield return c;
                }
            }
        }

        private IEnumerable<Coord> TestCoords(Coord orig, int dx, int dy, int dist)
        {
            for (int n = 1; n < Math.Max(Math.Min(Width, dist), Math.Min(Height, dist)); ++n)
            {
                var x = n * dx;
                var y = n * dy;
                var d = new Coord(x, y);
                var c = orig + d;
                if (!IsValid(c))
                    continue;
                yield return c;
            }
        }

        private IEnumerable<Coord> GetPossibleMovements(IPieceModel piece)
        {
            return null;
        }
        #endregion

        #region Private Fields
        private IReactiveCollection<IPieceModel> _pieces;
        #endregion
    }
}
