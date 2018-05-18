﻿using System;
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
    /// The main playing boardAgent. Can be of arbitrary dimention.
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

        public bool Create(int width, int height)
        {
            Width = width;
            Height = height;

            _contents = new List<List<Agent.ICardAgent>>();
            for (var n = 0; n < height; ++n)
            {
                var row = new List<Agent.ICardAgent>();
                for (var m = 0; m < width; ++m)
                    row.Add(null);
                _contents.Add(row);
            }

            return true;
        }

        public bool IsValid(Coord coord)
        {
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }

        public bool CanPlaceCard(Agent.ICardAgent cardAgent, Coord coord)
        {
            Assert.IsNotNull(cardAgent);
            Assert.IsTrue(IsValid(coord));

            // can play on empty square...
            var existing = At(coord);
            if (existing == null)
            {
                // ...unless within 4 squares of enemy king
                var adj = GetAdjacent(coord, Parameters.EnemyKingClosestPlacement).ToArray();
                return
                    adj.Length == 0 ||
                    adj.Any(c => c.CardAgent.Type == ECardType.King && !c.CardAgent.SameOwner(cardAgent.Owner));
            }

            // this is actually a battle
            if (!existing.SameOwner(cardAgent.Owner))
                return true;

            // true if we can mount an existing card there
            var mountable = cardAgent as Common.IMountable;
            if (mountable != null)
                return mountable.CanMount(cardAgent);

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

        public IEnumerable<Agent.ICardAgent> AttackedCards(Coord coord)
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

        public string ToString(Func<Coord, string> fun)
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

        public string CardToRep(Agent.ICardAgent cardAgent)
        {
            if (cardAgent == null) return "  ";
            var ch = $"{cardAgent.Model.ModelTemplate.Type.ToString()[0]} ";
            return ch;
        }

        public override string ToString()
        {
            return ToString((c) =>
            {
                var card = At(c);
                if (card == null)
                    return "  ";
                return CardToRep(card);
            });
        }

        private IEnumerable<Coord> GetPossibleMovements(Agent.ICardAgent cardAgent, Coord coord)
        {
            return null;
        }

        public IEnumerable<Agent.ICardAgent> DefendededCards(Agent.ICardAgent defender, Coord cood)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent.ICardAgent> Defenders(Coord cood)
        {
            throw new NotImplementedException();
        }

        public void NewGame()
        {
            Create(Width, Height);
        }

        public Agent.ICardAgent GetContents(Coord coord)
        {
            return !IsValidCoord(coord) ? null : At(coord);
        }

        public Agent.ICardAgent At(Coord coord)
        {
            var valid = IsValidCoord(coord);
            Assert.IsTrue(valid);
            return !valid ? null : _contents[coord.y][coord.x];
        }

        public Agent.ICardAgent At(int x, int y)
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

        public IEnumerable<Agent.ICardAgent> GetContents()
        {
            return _contents.SelectMany(row => row);
        }

        public void PlaceCard(Agent.ICardAgent cardAgent, Coord coord)
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
        private List<List<Agent.ICardAgent>> _contents;
        #endregion
    }
}
