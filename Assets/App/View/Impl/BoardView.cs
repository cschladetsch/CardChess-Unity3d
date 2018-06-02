using System.Collections.Generic;
using App.Common;
using UniRx;
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
        public IReadOnlyReactiveProperty<SquareView> SelectedSquare => _selectedSquare;

        [ContextMenu("Board-Clear")]
        protected override bool Create()
        {
            foreach (Transform tr in transform)
            {
                Destroy(tr.gameObject);
            }

            _squareBitMask = LayerMask.GetMask("BoardSquare");
            _selectedSquare.DistinctUntilChanged().Subscribe(
                sq => Info($"Over {sq}"));
            return true;
        }

        public override bool Construct(IBoardAgent agent)
        {
            if (!base.Construct(agent))
                return false;

            var board = Agent.Model;
            _width = board.Width;
            _height = board.Height;

            return CreateBoard();
       }

        [ContextMenu("Board-Create")]
        bool CreateBoard()
        {
            Destroy(_boardRoot);
            _boardRoot = new GameObject("Root");
            _boardRoot.transform.SetParent(gameObject.transform);

            var length = BlackPrefab.Length;
            Assert.AreEqual(BlackPrefab.Length, WhitePrefab.Length);
            var z = 0.0f;
            var origin = new Vector3(-length*(_width/2.0f - 1/2.0f), -length*(_height/2.0f - 1/2.0f), 0);
            var c = 1;
            _squares = new List<SquareView>(_width * _height);
            for (var ny = 0; ny < _height; ++ny)
            {
                for (var nx = 0; nx < _width; ++nx)
                {
                    var prefab = ((c + nx) % 2) == 1 ? WhitePrefab : BlackPrefab;
                    var square = Instantiate(prefab);
                    Assert.IsNotNull(square.GetComponent<Collider>());
                    var pos = origin + new Vector3(nx * length, ny * length, z);
                    square.transform.localPosition = Vector3.zero;
                    square.transform.SetParent(_boardRoot.transform);
                    square.transform.position = pos;
                    square.Coord = new Coord(nx, ny);

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

        protected override void Step()
        {
            base.Step();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, _squareBitMask))
            {
                var square = hit.transform.gameObject.GetComponent<SquareView>();
                _selectedSquare.Value = square;
            }
            else
            {
                _selectedSquare.Value = null;
            }
        }

        public SquareView At(Coord c)
        {
            return At(c.x, c.y);
        }

        public void Place(IPieceView piece)
        {
        }

        private List<SquareView> _squares;
        private GameObject _boardRoot;
        private int _width = 4, _height = 4;
        private int _squareBitMask;
        private readonly ReactiveProperty<SquareView> _selectedSquare = new ReactiveProperty<SquareView>();
    }
}
