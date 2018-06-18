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
        public int Width { get; set; }
        public int Height { get; set; }
        public IReadOnlyReactiveDictionary<Coord, IPieceModel> Pieces => _pieces;
        [Inject] public IArbiterModel Arbiter { get; set; }

        public override bool IsValid
        {
            get
            {
                Info("Testing Valid board");
                if (!base.IsValid) return false;
                if (Arbiter == null) return false;
                if (Width < 2) return false;
                if (Height < 2) return false;
                foreach (var kv in _pieces)
                {
                    var k = kv.Key;
                    var p = kv.Value;
                    if (p == null)
                        return false;
                    if (p.Coord.Value != k)
                        return false;
                }
                return true;
            }
        }

        public BoardModel(int width, int height)
            : base(null)
        {
            Width = width;
            Height = height;
        }

        public override void PrepareModels()
        {
            base.PrepareModels();
        }

        public override void StartGame()
        {
            ClearBoard();
            ConstructBoard();
        }

        public override void EndGame()
        {
            ClearBoard();
        }

        public IEnumerable<IPieceModel> PiecesOfType(EPieceType type)
        {
            return Pieces.Where(p => p.Value.PieceType == type).Select(kv => kv.Value);
        }

        public int NumPieces(EPieceType type)
        {
            return PiecesOfType(type).Count();
        }

        public IResponse Remove(IPieceModel pieceModel)
        {
            Assert.IsNotNull(pieceModel);
            IPieceModel existing = null;
            var oldCoord = pieceModel.Coord.Value;
            if (!_pieces.TryGetValue(oldCoord, out existing))
                return Response.Fail;
            Assert.AreSame(pieceModel, existing);
            _pieces.Remove(oldCoord);
            return Response.Ok;
        }

        public IResponse Move(IPieceModel pieceModel, Coord coord)
        {
            var removed = Remove(pieceModel);
            if (removed.Failed)
                return removed;
            Set(coord, pieceModel);
            return Response.Ok;
        }

        public IPieceModel RemovePiece(Coord coord)
        {
            if (!IsValidCoord(coord))
            {
                Warn($"Attempt to remove from invalid {coord}");
                return null;
            }
            var current = At(coord);
            return _pieces.Remove(coord) ? current : null;
        }

        public bool IsValidCoord(Coord coord)
        {
            Assert.IsNotNull(coord);
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }

        public IResponse TryMovePiece(MovePiece move)
        {
            Assert.IsNotNull(move);
            Assert.IsNotNull(move.Coord);
            Assert.IsNotNull(move.Piece);

            var coord = move.Coord;
            var piece = move.Piece;

            var dest = At(coord);
            if (dest != null)
            {
                if (dest != piece)
                    return Failed(move, $"Cannot move {piece} onto {dest}");
                return Response.Ok;
            }

            var movements = GetMovements(piece.Coord.Value).ToList();
            if (movements.Count == 0)
                return Failed(move, "No valid coords");

            if (!movements.Select(c => c != move.Coord).Any())
                return Failed(move, $"Cannot move {move.Piece} move to {move.Coord}");

            return MovePieceTo(coord, piece);
        }

        private IResponse MovePieceTo(Coord coord, IPieceModel piece)
        {
            var old = piece.Coord.Value;
            var resp = Set(coord, piece);
            if (resp.Success)
            {
                piece.Coord.Value = coord;
                resp = Set(old, null);
            }
            return resp;
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

            foreach (var c in GetAttacks(piece))
            {
                var attacked = At(c);
                if (attacked != null)
                    yield return attacked;
            }
        }

        public IEnumerable<IPieceModel> DefendededCards(IPieceModel defender, Coord cood)
        {
            Assert.IsNotNull(defender);
            NotImplemented("GetDefended");
            yield break;
        }

        public IEnumerable<IPieceModel> Defenders(Coord coord)
        {
            Assert.IsNotNull(coord);
            NotImplemented("Defenders");
            yield break;
        }

        public IPieceModel GetContents(Coord coord)
        {
            return !IsValidCoord(coord) ? null : At(coord);
        }

        public IPieceModel At(Coord coord)
        {
            Assert.IsTrue(IsValidCoord(coord));
            IPieceModel piece;
            return _pieces.TryGetValue(coord, out piece) ? piece : null;
        }

        public IPieceModel At(int x, int y)
        {
            return At(new Coord(x, y));
        }

        public IResponse<IPieceModel> TryPlacePiece(PlacePiece place)
        {
            Assert.IsNotNull(place);
            var coord = place.Coord;
            Assert.IsTrue(IsValidCoord(coord));

            if (At(coord) != null)
                return new Response<IPieceModel>(
                    null, EResponse.Fail, EError.InvalidTarget, $"Already {At(coord)}, cannot place {place.Card}");

            var piece = Registry.New<IPieceModel>(place.Player, place.Card);
            var set = Set(coord, piece);
            if (set.Success)
                piece.MovedThisTurn = true;

            var response = new Response<IPieceModel>(piece, set.Type, set.Error);
            Verbose(20, $"{place} -> {response}");
            return response;
        }

        public IResponse Remove(PieceModel piece)
        {
            Assert.IsNotNull(piece);
            return Set(piece.Coord.Value, null);
        }

        public IEnumerable<Coord> GetMovements(Coord coord)
        {
            Assert.IsNotNull(coord);
            Assert.IsTrue(IsValidCoord(coord));
            return GetMovements(At(coord));
        }

        public IEnumerable<Coord> GetAttacks(Coord coord)
        {
            return GetAttacks(At(coord));
        }

        public IEnumerable<Coord> GetMovements(IPieceModel piece)
        {
            if (piece == null)
            {
                Warn($"Attempt to get movements for empty piece");
                yield break;
            }
            var coord = piece.Coord.Value;
            foreach (var c in GetMovements(coord, piece.PieceType))
                yield return c;
        }

        public IEnumerable<Coord> GetAttacks(IPieceModel piece)
        {
            if (piece == null)
                yield break;
            foreach (var c in GetAttacks(piece.Coord.Value, piece.PieceType))
                yield return c;
        }

        public IEnumerable<Coord> GetAttacks(Coord coord, EPieceType type)
        {
            switch (type)
            {
                case EPieceType.King:
                case EPieceType.Queen:
                case EPieceType.Archer:
                case EPieceType.Castle:
                case EPieceType.Gryphon:
                    foreach (var c in GetMovements(coord, type).Where(IsValidCoord))
                        yield return c;
                    break;
                case EPieceType.Peon:
                    foreach (var c in Diagonals(coord, 1).Where(IsValidCoord))
                        yield return c;
                    break;
                case EPieceType.Siege:
                    foreach (var c in Orthogonals(coord, 2).Where(IsValidCoord))
                        yield return c;
                    break;
                case EPieceType.Barricade:
                    break;
                case EPieceType.None:
                    break;
                case EPieceType.Paladin:
                    break;
                case EPieceType.Priest:
                    break;
                case EPieceType.Ballista:
                    break;
                case EPieceType.Dragon:
                {
                    // can attack all adjacent squares
                    var offsets = new[]
                    {
                        new Coord(-1, 1),
                        new Coord(0, 1),
                        new Coord(1, 1),
                        new Coord(-1, 0),
                        new Coord(1, 0),
                        new Coord(-1, -1),
                        new Coord(0, -1),
                        new Coord(1, -1),
                    };
                    foreach (var off in offsets)
                    {
                        var c = coord + off;
                        if (IsValidCoord(c))
                            yield return c;
                    }
                    break;
                }
            }
        }

        public IEnumerable<Coord> GetMovements(Coord coord, EPieceType type)
        {
            var max = Math.Max(Width, Height);
            switch (type)
            {
                case EPieceType.King:
                    foreach (var c in Nearby(coord, 1))
                        yield return c;
                    break;
                case EPieceType.Peon:
                    foreach (var c in Orthogonals(coord, 1))
                        yield return c;
                    break;
                case EPieceType.Archer:
                    foreach (var c in Diagonals(coord, max))
                        yield return c;
                    break;
                case EPieceType.Gryphon:
                    {
                        var offsets = new[]
                        {
                        new Coord(-1, 2),
                        new Coord(-1, -2),
                        new Coord(1, -2),
                        new Coord(1, 2),
                        new Coord(-2, 1),
                        new Coord(-2, -1),
                        new Coord(2, 1),
                        new Coord(2, -1),
                    };
                        foreach (var c in offsets)
                        {
                            var d = coord + c;
                            if (IsValidCoord(d))
                                yield return d;
                        }
                        break;
                    }
                case EPieceType.Queen:
                    {
                        foreach (var c in Diagonals(coord, max).Concat(Orthogonals(coord, max)).Where(IsValidCoord))
                            yield return c;
                        break;
                    }
                case EPieceType.Siege:
                    // can't move
                    break;
                case EPieceType.Ballista:
                    foreach (var c in Nearby(coord, 1))
                        yield return c;
                    break;
                case EPieceType.Barricade:
                    // can't move
                    break;
                case EPieceType.None:
                    // can't move
                    break;
                case EPieceType.Paladin:
                    // unsure
                    break;
                case EPieceType.Priest:
                    // unsure
                    break;
                case EPieceType.Castle:
                    foreach (var c in Orthogonals(coord, max).Where(IsValidCoord))
                        yield return c;
                    break;
                case EPieceType.Dragon:
                {
                    // can move in a diamond pattern
                    var offsets = new[]
                    {
                        new Coord(0, 2),
                        new Coord(-1, 1),
                        new Coord(0, 1),
                        new Coord(1, 1),
                        new Coord(-2, 0),
                        new Coord(-1, 0),
                        new Coord(1, 0),
                        new Coord(2, 0),
                        new Coord(-1, -1),
                        new Coord(0, -1),
                        new Coord(1, -1),
                        new Coord(1, -1),
                        new Coord(0, -2),
                    };
                    foreach (var off in offsets)
                    {
                        var c = coord + off;
                        if (IsValidCoord(c))
                            yield return c;
                    }
                    break;
                }
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
            for (var y = Height - 1; y >= 0; --y)
            {
                // vertical axis
                sb.Append($" {y}:");

                // horizontal cards
                for (var x = 0; x < Width; ++x)
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
                    for (var x = 0; x < Width; ++x)
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

        private void ConstructBoard()
        {
            _pieces.Clear();
        }

        private IResponse Set(Coord coord, IPieceModel piece)
        {
            Assert.IsTrue(IsValidCoord(coord));
            _pieces[coord] = piece;
            return Response.Ok;
        }

        private void ClearBoard()
        {
            if (_pieces == null)
                return;

            foreach (var kv in _pieces)
            {
                kv.Value.Destroy();
            }
            _pieces.Clear();
        }

        private IEnumerable<Coord> Nearby(Coord orig, int dist)
        {
            var y = Math.Max(orig.y - dist, 0);
            for (; y <= orig.y + dist; ++y)
            {
                for (var x = Math.Max(orig.x - dist, 0); x <= orig.x + dist; ++x)
                {
                    var coord = new Coord(x, y);
                    if (!IsValidCoord(coord))
                        break;
                    if (!Equals(coord, orig))
                        yield return coord;
                }
            }
        }

        private IEnumerable<Coord> Orthogonals(Coord orig, int dist)
        {
            for (var dx = -dist; dx <= dist; ++dx)
            {
                var p0 = new Coord(orig.x + dx, orig.y);
                if (IsValidCoord(p0) && dx != 0)
                    yield return p0;
            }
            for (var dy = -dist; dy <= dist; dy++)
            {
                var p1 = new Coord(orig.x, orig.y + dy);
                if (IsValidCoord(p1) && dy != 0)
                    yield return p1;
            }
        }

        private IEnumerable<Coord> Diagonals(Coord orig, int dist)
        {
            for (int y = -dist; y <= dist; ++y)
            {
                foreach (var p in new[] {-y, y})
                {
                    var c = orig + new Coord(p, y);
                    if (IsValidCoord(c))
                        yield return c;
                }
            }
        }

        public IResponse Add(IPieceModel piece)
        {
            Assert.IsNull(At(piece.Coord.Value));
            Set(piece.Coord.Value, piece);
            return Response.Ok;
        }


        public void NewTurn()
        {
            foreach (var p in _pieces.Values)
                p.NewTurn();
        }

        private readonly ReactiveDictionary<Coord, IPieceModel> _pieces = new ReactiveDictionary<Coord, IPieceModel>();
    }
}
