using System;
using System.Collections.Generic;
using System.Linq;
using App.Action;
using UnityEngine.Assertions;

namespace App.Model
{
    /// <summary>
    /// The main playing board. Can be of arbitrary dimention.
    /// Contents are stored as row-major. the bottom left corner for white is at contents[0][0]
    /// the topright corner for white is at contents[Height - 1][Width - 1]
    /// Both Black and White use the same coordinate system.
    /// </summary>
    public class Board : ModelBase, IBoard
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool Create(int width, int height)
        {
            Width = width;
            Height = height;

            _contents = new List<List<ICardInstance>>();
            for (var n = 0; n < height; ++n)
            {
                var row = new List<ICardInstance>();
                for (var m = 0; m < width; ++m)
                    row.Add(null);
                _contents.Add(row);
            }

            return true;
        }

        public void NewGame()
        {
            Create(Width, Height);
        }

        public ICardInstance GetContents(Coord coord)
        {
            return !IsValidCoord(coord) ? null : At(coord);
        }

        public ICardInstance At(Coord coord)
        {
            var valid = IsValidCoord(coord);
            Assert.IsTrue(valid);
            return !valid ? null : _contents[coord.Y][coord.X];
        }

        public bool IsValidCoord(Coord coord)
        {
            return coord.X >= 0 && coord.Y >= 0 && coord.X < Width && coord.Y < Height;
        }

        public IEnumerable<ICardInstance> GetContents()
        {
            return _contents.SelectMany(row => row);
        }

        private List<List<ICardInstance>> _contents;
    }
}

