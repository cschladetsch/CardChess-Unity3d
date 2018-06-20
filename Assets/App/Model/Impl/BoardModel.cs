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
        public IReadOnlyReactiveCollection<IPieceModel> Pieces => _pieces;
        [Inject] public IArbiterModel Arbiter { get; set; }

        public override bool IsValid
        {
            get
            {
                if (!base.IsValid) return false;
                if (Arbiter == null) return false;
                if (Width < 2) return false;
                if (Height < 2) return false;
                foreach (var p in _pieces)
                {
                    if (p == null)
                        return false;
                    if (!IsValidCoord(p.Coord.Value))
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
            return Pieces.Where(p => p.PieceType == type);
        }

        public int NumPieces(EPieceType type)
        {
            return PiecesOfType(type).Count();
        }

        public IResponse Remove(IPieceModel pieceModel)
        {
            Assert.IsNotNull(pieceModel);
            return _pieces.Remove(pieceModel) ? Response.Ok : Response.Fail;
        }

        public IResponse Move(IPieceModel pieceModel, Coord coord)
        {
            Assert.IsNotNull(pieceModel);
            Assert.IsNotNull(coord);
            Assert.IsTrue(IsValidCoord(coord));
            var found = Get(coord);
            if (found == null)
                return Response.Fail;
            found.Coord.Value = coord;
            return Response.Ok;
        }

        IPieceModel Get(Coord coord)
        {
            return _pieces.FirstOrDefault(p => p.Coord.Value == coord);
        }

        public IPieceModel RemovePiece(Coord coord)
        {
            Assert.IsTrue(IsValidCoord(coord));
            var current = At(coord);
            Assert.IsNotNull(current);
            Assert.IsTrue(_pieces.Remove(current));
            return current;
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
                return dest != piece ? Failed(move, $"Cannot move {piece} onto {dest}") : Response.Ok;
            }

            var movements = GetMovements(piece);
            if (!movements.Coords.Any())
                return Failed(move, $"Cannot move {move.Piece} move to {move.Coord}");
            return MovePieceTo(coord, piece);
        }

        private IResponse MovePieceTo(Coord coord, IPieceModel piece)
        {
            Assert.IsNotNull(piece);
            Assert.IsNotNull(coord);
            Assert.IsTrue(IsValidCoord(coord));
            var found = _pieces.FirstOrDefault(p => p == piece);
            Assert.IsNotNull(found);
            if (found == null)
                return Response.Fail;
            found.Coord.Value = coord;
            return Response.Ok;
        }

        public IPieceModel At(Coord coord)
        {
            Assert.IsTrue(IsValidCoord(coord));
            return _pieces.FirstOrDefault(p => p.Coord.Value == coord);
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
            var set = AddPiece(coord, piece);
            if (set.Success)
                piece.MovedThisTurn = true;

            var response = new Response<IPieceModel>(piece, set.Type, set.Error);
            Verbose(20, $"{place} -> {response}");
            return response;
        }

        public IResponse Remove(PieceModel piece)
        {
            Assert.IsNotNull(piece);
            return AddPiece(piece.Coord.Value, null);
        }

        public string Print()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"BoardModel with {_pieces.Count} pieces");
            foreach (var p in _pieces)
                sb.AppendLine($"{p.Coord.Value} => {p}");
            sb.AppendLine();
            sb.AppendLine(Print(coord =>
            {
                var piece = At(coord);
                var rep = CardToRep(piece.Card);
                var black = piece.Owner.Value.Color != EColor.White;
                if (black)
                    rep = rep.ToLower();
                return rep;
            }));
            return sb.ToString();
        }

        public string Print(Func<Coord, string> fun)
        {
            var sb = new StringBuilder();
            for (var y = Height - 1; y >= 0; --y)
            {
                sb.Append($" {y}:");
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
            Assert.IsTrue(IsValid);
            foreach (var kv in _pieces)
                kv.Destroy();
            _pieces.Clear();
        }

        private IResponse AddPiece(Coord coord, IPieceModel piece)
        {
            Assert.IsNotNull(piece);
            Assert.IsTrue(IsValidCoord(coord));
            piece.Coord.Value = coord;
            _pieces.Add(piece);
            return Response.Ok;
        }

        private void ClearBoard()
        {
            if (_pieces == null)
                return;

            foreach (var kv in _pieces)
            {
                kv.Destroy();
            }
            _pieces.Clear();
        }

        public IResponse Add(IPieceModel piece)
        {
            Assert.IsNull(At(piece.Coord.Value));
            AddPiece(piece.Coord.Value, piece);
            return Response.Ok;
        }

        public void NewTurn()
        {
            Assert.IsTrue(IsValid);
            foreach (var p in _pieces)
            {
                Assert.IsNotNull(p);
                p.NewTurn();
            }
        }

        public MoveResults GetMovements(Coord coord)
        {
            Assert.IsNotNull(coord);
            Assert.IsTrue(IsValidCoord(coord));
            return GetMovements(At(coord));
        }

        public MoveResults GetAttacks(Coord coord)
        {
            return GetAttacks(At(coord));
        }

        public MoveResults GetMovements(IPieceModel piece)
        {
            if (piece == null)
            {
                Warn($"Attempt to get movements for empty piece");
                return null;
            }
            var coord = piece.Coord.Value;
            return GetMovements(coord, piece.PieceType);
        }

        public MoveResults GetAttacks(IPieceModel piece)
        {
            return piece == null ? null : GetAttacks(piece.Coord.Value, piece.PieceType);
        }

        public MoveResults GetAttacks(Coord coord, EPieceType type)
        {
            switch (type)
            {
                case EPieceType.King:
                case EPieceType.Queen:
                case EPieceType.Archer:
                case EPieceType.Castle:
                case EPieceType.Gryphon:
                    return GetMovements(coord, type);
                case EPieceType.Peon:
                    return Diagonals(coord, 1);
                case EPieceType.Siege:
                    return null;
                case EPieceType.Dragon:
                    return GetMoveResults(coord, 2, _surrounding);
            }

            return null;
        }

        public MoveResults GetMovements(Coord coord, EPieceType type)
        {
            var max = Math.Max(Width, Height);
            switch (type)
            {
                case EPieceType.King:
                    return GetMoveResults(coord, 1, _surrounding);
                case EPieceType.Peon:
                    return GetMoveResults(coord, 1, _orthogonals);
                case EPieceType.Archer:
                    return GetMoveResults(coord, max, _diagnonals);
                case EPieceType.Gryphon:
                    return GetMoveResults(coord, 1, _knightMoves);
                case EPieceType.Queen:
                    return GetMoveResults(coord, max, _orthogonals.Concat(_diagnonals).ToArray());
                case EPieceType.Siege:
                    break;
                case EPieceType.Ballista:
                    return GetMoveResults(coord, 2, _orthogonals);
                case EPieceType.Barricade:
                    break;
                case EPieceType.None:
                    break;
                case EPieceType.Paladin:
                    break;
                case EPieceType.Priest:
                    break;
                case EPieceType.Castle:
                    return GetMoveResults(coord, max, _orthogonals);
                case EPieceType.Dragon:
                    return GetMoveResults(coord, 1, _diamond);
            }

            return null;
        }

        private MoveResults Diagonals(Coord orig, int dist)
        {
            return GetMoveResults(orig, dist, _diagnonals);
        }

        MoveResults GetMoveResults(Coord orig, int dist, Coord[] dirs)
        {
            var moveResults = new MoveResults();
            var blocked = new List<int>();
            for (int n = 1; n <= dist; ++n)
            {
                for (int m = 0; m < dirs.Length; ++m)
                {
                    if (blocked.Contains(m))
                        continue;
                    var next = dirs[m];
                    var coord = orig + next*n;
                    if (coord == orig)
                        continue;
                    if (!IsValidCoord(coord))
                        continue;
                    var model = At(coord);
                    if (model != null)
                    {
                        moveResults.Interferernce.Add(model);
                        blocked.Add(m);
                        continue;
                    }

                    moveResults.Coords.Add(coord);
                }
            }
            return moveResults;
        }

        private readonly Coord[] _surrounding = {
            new Coord(-1, 1),
            new Coord(0, 1),
            new Coord(1, 1),
            new Coord(-1, 0),
            new Coord(0, 0),
            new Coord(1, 0),
            new Coord(-1, -1),
            new Coord(0, -1),
            new Coord(1, -1),
        };

        private readonly Coord[] _knightMoves = {
            new Coord(-1, 2),
            new Coord(-1, -2),
            new Coord(1, -2),
            new Coord(1, 2),
            new Coord(-2, 1),
            new Coord(-2, -1),
            new Coord(2, 1),
            new Coord(2, -1),
        };

        private readonly Coord[] _orthogonals = {
            new Coord(0, 1),
            new Coord(1, 0),
            new Coord(0, -1),
            new Coord(-1, 0),
        };

        private readonly Coord[] _diagnonals = {
            new Coord(-1, 1),
            new Coord(1, 1),
            new Coord(-1, -1),
            new Coord(1, -1),
        };

        private readonly Coord[] _diamond = {
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

        private readonly ReactiveCollection<IPieceModel> _pieces = new ReactiveCollection<IPieceModel>();
    }
}
