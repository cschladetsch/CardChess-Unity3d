using System.Collections.Generic;
using System.Linq;
using App.Model;
using CoLib;
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
        #region Unityu Properties
        public Material BlackPieceMaterial;
        public Material WhitePieceMaterial;
        public PieceView PieceViewPrefab;
        public SquareView BlackPrefab;
        public SquareView WhitePrefab;
        public Transform SquaresRoot;
        public Transform PiecesRoot;
        public int BoardWidth;
        public int BoardHeight;
        public BoardOverlayView OverlayView;
        #endregion

        public Material BlackMaterial => BlackPieceMaterial;
        public Material WhiteMaterial => WhitePieceMaterial;
        public IReadOnlyReactiveProperty<ISquareView> HoverSquare => _hoverSquare;
        public IReadOnlyReactiveProperty<IPieceView> HoverPiece => _hoverPiece;
        public IReadOnlyReactiveProperty<int> Width => Agent.Width;
        public IReadOnlyReactiveProperty<int> Height => Agent.Height;

        public override void Create()
        {
            _squareBitMask = LayerMask.GetMask("BoardSquare");
            _hoveredSquare.DistinctUntilChanged().Subscribe(sq => _hoverSquare.Value = sq);
            HoverSquare.Subscribe(sq =>
            {
                OverlayView.Clear();
                if (sq == null) return;
                var p = Agent.At(sq.Coord);
                if (p == null) return;
                Assert.AreEqual(sq.Coord, p.Coord.Value);
                Info($"{sq} has {p} @{p.Coord}");
                var movements = Agent.Model.GetMovements(sq.Coord).ToList();
                OverlayView.Add(movements, Color.green);
            });
        }

        public override void SetAgent(IPlayerView view, IBoardAgent agent)
        {
            Assert.IsNotNull(agent);
            base.SetAgent(view, agent);
            Clear();
            CreateBoard();
        }

        public void ShowSquares(PieceView pieceView)
        {
        }

        public IPieceView Get(Coord coord)
        {
            return _pieceViews.FirstOrDefault(p => p.Coord.Value == coord);
        }

        public IPieceView PlacePiece(ICardView view, Coord coord)
        {
            Assert.IsNotNull(view);
            Assert.IsNotNull(coord);
            var pv = MakePieceView(view, coord);
            var tr = pv.GameObject.transform;
            tr.SetParent(PiecesRoot);
            tr.position = view.GameObject.transform.position;
            _pieceViews.Add(pv);
            return pv;
        }

        private IPieceView MakePieceView(ICardView cardView, Coord coord)
        {
            Assert.IsNotNull(cardView);
            Assert.IsNotNull(coord);
            var model = Agent.Model.Registry.New<IPieceModel>(cardView.PlayerModel, cardView.Agent.Model);
            var agent = Agent.Registry.New<IPieceAgent>(model);
            var view = ViewRegistry.FromPrefab<IPieceView>(PieceViewPrefab);
            view.SetAgent(cardView.PlayerView, agent);
            view.Coord.Value = coord;

            Assert.IsTrue(Agent.Add(view.Agent).Success);
            var barny = Agent.At(coord);
            Assert.AreSame(barny, model);

            Assert.IsTrue(view.IsValid);
            Assert.AreEqual(view.Coord.Value, coord);
            Assert.AreSame(agent, view.Agent);
            Assert.AreEqual(model.Coord.Value, coord);
            Assert.AreEqual(agent.Coord.Value, coord);

            var fred = Agent.At(coord);
            Assert.AreSame(fred, agent);
            Assert.AreSame(fred, view.Agent);
            Assert.AreEqual(fred.Coord.Value, coord);
            return view;
        }

        [ContextMenu("Board-Clear")]
        public void Clear()
        {
            foreach (Transform tr in SquaresRoot.transform)
                Destroy(tr.gameObject);
            foreach (Transform tr in PiecesRoot.transform)
                Destroy(tr.gameObject);
        }

        [ContextMenu("Board-Create")]
        void CreateBoard()
        {
            Clear();
            var length = BlackPrefab.Length;
            Assert.AreEqual(BlackPrefab.Length, WhitePrefab.Length);
            int width = Width.Value;
            int height = Height.Value;
            var z = 0.0f;
            var origin = new Vector3(-length*(width/2.0f - 1/2.0f), -length*(height/2.0f - 1/2.0f), 0);
            var c = 1;
            _squares = new List<SquareView>(width * height);
            for (var ny = 0; ny < height; ++ny)
            {
                for (var nx = 0; nx < width; ++nx)
                {
                    var white = ((c + nx) % 2) == 1;
                    var prefab = white ? WhitePrefab : BlackPrefab;
                    var square = Instantiate(prefab);
                    Assert.IsNotNull(square.GetComponent<Collider>());
                    var pos = origin + new Vector3(nx * length, ny * length, z);
                    square.transform.localPosition = Vector3.zero;
                    square.transform.SetParent(SquaresRoot.transform);
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
            Assert.IsTrue(x >= 0 && x < Width.Value);
            Assert.IsTrue(y >= 0 && x < Height.Value);
            return _squares[y * Width.Value + x];
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

        //public void Place(IPieceView piece)
        //{
        //}

        private int _squareBitMask;
        private List<SquareView> _squares;
        private readonly ReactiveProperty<ISquareView> _hoveredSquare = new ReactiveProperty<ISquareView>();
        private readonly ReactiveProperty<ISquareView> _hoverSquare = new ReactiveProperty<ISquareView>();
        private readonly ReactiveProperty<IPieceView> _hoverPiece = new ReactiveProperty<IPieceView>();
        private readonly List<IPieceView> _pieceViews = new List<IPieceView>();
    }
}
