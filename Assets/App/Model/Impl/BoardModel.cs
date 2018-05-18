using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using App.Action;
using App.Agent;
using UnityEngine.Assertions;

namespace App.Model
{
    using Common;

    // boards do not store model cards

    /// <summary>
    /// The main playing board Can be of arbitrary dimention.
    /// Contents are stored as row-major. the bottom left corner for white is at contents[0][0]
    /// the topright corner for white is at contents[Height - 1][Width - 1]
    /// Both Black and White use the same coordinate system.
    /// </summary>
    public class BoardModel :
        ModelBase
        , IBoardModel
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public BoardModel() { throw new NotImplementedException(); }

        public BoardModel(int width, int height)
        {
            Width = width;
            Height = height;

            ConstructBoard();
        }

        public void NewGame()
        {
            ClearBoard();
        }

        private void ClearBoard()
        {
            foreach (var card in GetContents())
                card.Destroy();
        }

        private void ConstructBoard()
        {
            _contents = new List<List<ICardModel>>();
            for (var n = 0; n < Height; ++n)
            {
                var row = new List<ICardModel>();
                for (var m = 0; m < Width; ++m)
                    row.Add(null);
                _contents.Add(row);
            }
        }

        public bool IsValid(Coord coord)
        {
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }

        public bool CanPlaceCard(ICardModel card, Coord coord)
        {
            Assert.IsNotNull(card);
            Assert.IsTrue(IsValid(coord));

            // can play on empty square...
            var existing = At(coord);
            if (existing == null)
            {
                // ...unless within 4 squares of enemy king
                var adj = GetAdjacent(coord, Parameters.EnemyKingClosestPlacement).ToArray();
                return
                    adj.Length == 0 ||
                    adj.Any(c => c.Card.Type == ECardType.King && !c.Card.SameOwner(card.Owner));
            }

            // this is actually a battle
            if (!existing.SameOwner(card.Owner))
                return true;

            // true if we can mount an existing card there
            var mountable = card as ICardModelMountable;
            if (mountable != null)
                return mountable.CanMount(card);

            return false;
        }

        public IEnumerable<PlayCard> GetAdjacent(Coord coord, int dist = 1)
        {
            var items = new List<PlayCard>();
            for (var y = -dist; y <= dist; ++y)
            {
                for (var x = -dist; x <= dist; ++x)
                {
                    if (x == 0 && y == 0)
                        continue;

                    var card = At(coord);
                    if (card == null)
                        continue;

                    items.Add(new PlayCard(card, coord));
                }
            }
            return items;
        }

        public IEnumerable<ICardModel> AttackedCards(Coord coord)
        {
            var card = At(coord);
            if (card == null)
                return null;
            var coords = GetPossibleMovements(card, coord);
            return null;
        }

        static int Max(int a, int b) { return a > b ? a : b; }

        IEnumerable<Coord> OrthoAndDiags(Coord orig)
        {
            return Orthogonals(orig).Concat(Diagonals(orig));
        }

        IEnumerable<Coord> Orthogonals(Coord orig)
        {
            for (var y = Max(orig.y - Height, 0); y < Height; ++y)
            {
                var coord = new Coord(orig.x, y);
                if (!IsValid(coord))
                    continue;
                if (!Equals(coord, orig))
                    yield return coord;
            }
            for (var x = Max(orig.x - Width, 0); x < Width; ++x)
            {
                var coord = new Coord(x, orig.y);
                if (!IsValid(coord))
                    continue;
                if (!Equals(coord, orig))
                    yield return coord;
            }
        }

        IEnumerable<Coord> TestCoords(Coord orig, int dx, int dy)
        {
            for (int n = 1; n < Max(Width, Height); ++n)
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

        IEnumerable<Coord> Diagonals(Coord orig)
        {
            for (int dx = -1; dx < 2; dx++)
            {
                if (dx == 0) continue;
                for (int dy = -1; dy < 2; dy++)
                {
                    if (dy == 0) continue;
                    foreach (var c in TestCoords(orig, dx, dy))
                        yield return c;
                }
            }
            yield break;
        }

        public string Print()
        {
            return Print((c) =>
            {
                var card = At(c);
                return card == null ? "  " : CardToRep(card);
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
                    var a = $"{fun(new Coord(x, y))}";
                    //Assert.AreEqual(2, a.Length);
                    sb.Append(a);
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

        public string CardToRep(ICardModel card)
        {
            if (card == null) return "  ";
            var ch = $"{card.ModelTemplate.Type.ToString()[0]} ";
            return ch;
        }

        private IEnumerable<Coord> GetPossibleMovements(ICardModel cardAgent, Coord coord)
        {
            return null;
        }

        public IEnumerable<ICardModel> DefendededCards(ICardModel defender, Coord cood)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICardModel> Defenders(Coord cood)
        {
            throw new NotImplementedException();
        }

        public ICardModel GetContents(Coord coord)
        {
            return !IsValidCoord(coord) ? null : At(coord);
        }

        public ICardModel At(Coord coord)
        {
            var valid = IsValidCoord(coord);
            Assert.IsTrue(valid);
            return !valid ? null : _contents[coord.y][coord.x];
        }

        public ICardModel At(int x, int y)
        {
            if (x < 0 || y < 0)
                return null;
            if (x >= Width || y >= Height)
                return null;
            return _contents[y][x];
        }

        public bool IsValidCoord(Coord coord)
        {
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }

        public IEnumerable<ICardModel> GetContents()
        {
            return _contents.SelectMany(row => row);
        }

        public void PlaceCard(ICardModel cardAgent, Coord coord)
        {
            Info("BoardAgent: Placed {card.Owner.Color} {card} at {coord}");
            _contents[coord.y][coord.x] = cardAgent;
        }

        public IEnumerable<Coord> GetMovements(Coord coord)
        {
            var card = _contents[coord.y][coord.x];
            if (card == null)
                return null;
            return Diagonals(coord);
        }

        #region Private Fields
        private List<List<ICardModel>> _contents;
        #endregion
    }
}

