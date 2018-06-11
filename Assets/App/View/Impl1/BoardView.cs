using System.Collections.Generic;
using UnityEngine;

using UniRx;

#pragma warning disable 649

namespace App.View.Impl1
{
    using Agent;
    using Common;

    /// <summary>
    /// View of the play board during a game.
    /// </summary>
    public class BoardView
        : ViewBase<IBoardAgent>
        , IBoardView
    {
        public SquareView BlackPrefab;
        public SquareView WhitePrefab;
        public int Width = 8;
        public int Height = 8;
        public Transform Root;
        public IReadOnlyReactiveProperty<ISquareView> HoverSquare => _hoverSquare;

        public override void Create()
        {
            _squareBitMask = LayerMask.GetMask("BoardSquare");

            _hoveredSquare.DistinctUntilChanged().Subscribe(sq => _hoverSquare.Value = sq);
            HoverSquare.Subscribe(sq =>
            {
                //if (sq != null)
                //    Info($"Over {sq}");//.AgentBase.BaseModel}");
            });

            Clear();
            CreateBoard();
        }

        public override void SetAgent(IPlayerView view, IBoardAgent agent)
        {
            base.SetAgent(view, agent);

            var board = Agent.Model;
            Width = board.Width;
            Height = board.Height;
        }

        [ContextMenu("Board-Clear")]
        public void Clear()
        {
            foreach (Transform tr in Root.transform)
            {
                Unity.Destroy(tr.gameObject);
            }
        }

        [ContextMenu("Board-Create")]
        void CreateBoard()
        {
            Clear();
            var length = BlackPrefab.Length;
            Assert.AreEqual(BlackPrefab.Length, WhitePrefab.Length);
            var z = 0.0f;
            var origin = new Vector3(-length*(Width/2.0f - 1/2.0f), -length*(Height/2.0f - 1/2.0f), 0);
            var c = 1;
            _squares = new List<SquareView>(Width * Height);
            for (var ny = 0; ny < Height; ++ny)
            {
                for (var nx = 0; nx < Width; ++nx)
                {
                    var white = ((c + nx) % 2) == 1;
                    var prefab = white ? WhitePrefab : BlackPrefab;
                    var square = Instantiate(prefab);
                    Assert.IsNotNull(square.GetComponent<Collider>());
                    var pos = origin + new Vector3(nx * length, ny * length, z);
                    square.transform.localPosition = Vector3.zero;
                    square.transform.SetParent(Root.transform);
                    square.transform.position = pos;
                    square.Coord = new Coord(nx, ny);
                    square.Color = white ? EColor.White : EColor.Black;

                    _squares.Add(square);
                }

                ++c;
            }
        }

        public SquareView At(int x, int y)
        {
            Assert.IsTrue(x >= 0 && x < Width);
            Assert.IsTrue(y >= 0 && x < Height);
            return _squares[y * Width + x];
        }

        protected override void Step()
        {
            base.Step();

            TestRayCast();
        }

        public ISquareView TestRayCast(Vector3 screen)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, _squareBitMask))
            {
                var square = hit.transform.gameObject.GetComponent<SquareView>();
                if (square != null)
                    return _hoveredSquare.Value = square;
            }
            else
            {
                return _hoveredSquare.Value = null;
            }

            return null;
        }

        private ISquareView TestRayCast()
        {
            return TestRayCast(Input.mousePosition);
        }

        public SquareView At(Coord c)
        {
            return At(c.x, c.y);
        }

        public void Place(IPieceView piece)
        {
        }

        private int _squareBitMask;
        private List<SquareView> _squares;
        private readonly ReactiveProperty<ISquareView> _hoveredSquare = new ReactiveProperty<ISquareView>();
        private readonly ReactiveProperty<ISquareView> _hoverSquare = new ReactiveProperty<ISquareView>();
    }
}
