using UniRx;

namespace App.View
{
    using Agent;
    using Common;

    public interface IPieceView
        : IView<IPieceAgent>
    {
        IReactiveProperty<Coord> Coord { get; }
    }
}
