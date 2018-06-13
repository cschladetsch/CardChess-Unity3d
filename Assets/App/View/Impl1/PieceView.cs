using App.Common;

namespace App.View.Impl1
{
    using Agent;

    public class PieceView
        : Draggable<IPieceAgent>
        , IPieceView
    {
        protected override void MouseDown()
        {
            Info($"MouseDown {this}");
        }

        protected override void MouseHover()
        {
        }

        protected override void MouseUp(IBoardView board, Coord coord)
        {
            ReturnToStart();
        }
    }
}
