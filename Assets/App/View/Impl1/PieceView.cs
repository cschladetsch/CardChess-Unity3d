using App.Common;
using CoLib;
using UniRx;
using UnityEngine;

namespace App.View.Impl1
{
    using Agent;

    /// <summary>
    /// View of a Piece on the Board in the scene.
    /// </summary>
    public class PieceView
        : Draggable<IPieceAgent>
        , IPieceView
    {
        public IReactiveProperty<Coord> Coord => Agent.Coord;

        protected override void Begin()
        {
            base.Begin();
            Coord.Subscribe(c => Move());
        }

        private void Move()
        {
            var coord = Coord.Value;
            var go = GameObject;
            var pos = new Vector3(coord.x - BoardView.Width.Value/2 + 0.5f, coord.y - BoardView.Height.Value/2 + 0.5f, -1);
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.MoveTo(go, pos, 0.1, Ease.InOutBounce(), true),
                    Commands.ScaleTo(go, 1, 0.1)
                )
            );
        }

        protected override bool MouseDown()
        {
            Info($"MouseDown {this}");
            return IsCurrentPlayer();
        }

        protected override void MouseHover()
        {
            Info($"MouseHover {this}");
        }

        protected override void MouseUp(IBoardView board, Coord coord)
        {
            // TODO: TryMovePiece
            // TODO: TryBattle
            ReturnToStart();
        }
    }
}
