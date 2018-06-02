using System.Collections.Generic;
using App.Common;
using UnityEngine;

#pragma warning disable 649

namespace App.View.Impl
{
    using Agent;

    class BoardView
        : ViewBase<IBoardAgent>
    {
        public SquareView BlackPrefab;
        public SquareView WhitePrefab;

        public override bool Construct(IBoardAgent agent)
        {
            if (!base.Construct(agent))
                return false;

            var board = agent.Model;
            _width = board.Width;
            _height = board.Height;
            var z = 0.0f;
            var origin = new Vector3(0,0,0);
            var c = 1;
            _squares = new List<SquareView>(_width * _height);
            for (var ny = 1; ny <= _height; ++ny)
            {
                for (var nx = 1; nx <= _width; ++nx)
                {
                    var prefab = (c + nx % 2) == 0 ? WhitePrefab : BlackPrefab;
                    var square = Instantiate(prefab);
                    var pos = origin + new Vector3(nx * prefab.Length, ny * prefab.Length, z);
                    square.transform.SetParent(gameObject.transform);
                    square.transform.position = pos;

                    _squares.Add(square);
                }

                ++c;
            }

            return true;
        }

        public SquareView At(int x, int y)
        {
            Assert.IsTrue(x >= 0 && x < _width);
            Assert.IsTrue(y >= 0 && x < _height);
            return _squares[y * _width + x];
        }

        public SquareView At(Coord c)
        {
            return At(c.x, c.y);
        }

        public void Place(IPieceView piece)
        {
        }

        private List<SquareView> _squares;
        private int _width, _height;
    }
}
