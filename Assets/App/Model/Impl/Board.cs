using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Action;
using UnityEngine.Assertions;

namespace App.Model
{
    using Common;

    // boards do not store model cards
    using ICard = Agent.ICardInstance;

    /// <summary>
    /// The main playing board. Can be of arbitrary dimention.
    /// Contents are stored as row-major. the bottom left corner for white is at contents[0][0]
    /// the topright corner for white is at contents[Height - 1][Width - 1]
    /// Both Black and White use the same coordinate system.
    /// </summary>
    public class Board :
        ModelBase
        , IBoard
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool Create(int width, int height)
        {
            Width = width;
            Height = height;

            _contents = new List<List<ICard>>();
            for (var n = 0; n < height; ++n)
            {
                var row = new List<ICard>();
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

        public bool CanPlaceCard(ICard card, Coord coord)
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
            var mountable = card as Common.IMountable;
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

        public IEnumerable<ICard> AttackedCards(Coord coord)
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

        IEnumerable<Coord> Diagonals(Coord orig)
        {
            yield break;
        }

        private IEnumerable<Coord> GetPossibleMovements(ICard card, Coord coord)
        {
            return null;
        }

        public IEnumerable<ICard> DefendededCards(ICard defender, Coord cood)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICard> Defenders(Coord cood)
        {
            throw new NotImplementedException();
        }

        public void NewGame()
        {
            Create(Width, Height);
        }

        public ICard GetContents(Coord coord)
        {
            return !IsValidCoord(coord) ? null : At(coord);
        }

        public ICard At(Coord coord)
        {
            var valid = IsValidCoord(coord);
            Assert.IsTrue(valid);
            return !valid ? null : _contents[coord.y][coord.x];
        }

        public bool IsValidCoord(Coord coord)
        {
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }

        public IEnumerable<ICard> GetContents()
        {
            return _contents.SelectMany(row => row);
        }

        public void PlaceCard(ICard card, Coord coord)
        {
            _contents[coord.y][coord.x] = card;
        }

        public IEnumerable<Coord> GetMovements(Coord coord)
        {
            var card = _contents[coord.y][coord.x];
            if (card == null)
                return null;
            return Diagonals(coord);
        }

        #region Private Fields
        private List<List<ICard>> _contents;
        private IDictionary<ECardType, IEnumerator<Coord>> _typeToCoords = new ConcurrentDictionary<ECardType, IEnumerator<Coord>>( );
        #endregion
    }
}

