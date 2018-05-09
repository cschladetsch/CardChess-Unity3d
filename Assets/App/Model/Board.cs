using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Flow;

namespace App.Model
{
    /// <summary>
    /// The main playing board. Can be of arbitrary dimention.
    /// Contents are stored as row-major. the bottom left corner for white is at contents[0][0]
    /// the topright corner for white is at contents[Height - 1][Width - 1]
    /// Both Black and White use the same coordinate system.
    /// </summary>
    public class Board : ModelBase
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float SquareSideLength = 2;

        public void Create(int width, int height)
        {
            _contents = new List<List<IInstance>>();
            for (var n = 0; n < height; ++n)
            {
                var row = new List<IInstance>();
                for (var m = 0; m < width; ++m)
                    row.Add(null);
                _contents.Add(row);
            }

            Width = width;
            Height = height;
        }

        public IInstance GetContents(Coord coord)
        {
            if (!IsValid(coord))
                return null;
            return At(coord);
        }

        private IInstance At(Coord coord)
        {
            return _contents[coord.Y][coord.X];
        }

        public bool IsValid(Coord coord)
        {
            return coord.X >= 0 && coord.Y >= 0 && coord.X < Width && coord.Y < Height;
        }

        public IEnumerable<IInstance> GetContents()
        {
            return _contents.SelectMany(row => row);
        }

        private List<List<IInstance>> _contents;
    }
}

