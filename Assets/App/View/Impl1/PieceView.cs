using App.Common;

namespace App.View.Impl1
{
    using Agent;

    public class PieceView
        : Draggable<IPieceAgent>
        , IPieceView
    {
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
            // TODO: Battles
            ReturnToStart();
        }
    }
}
