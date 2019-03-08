using Dekuple.View;
using UniRx;

namespace App.View
{
    using Agent;
    using Common;

    public interface IPieceView
        : IView<IPieceAgent>
    {
        IReactiveProperty<Coord> Coord { get; }
        IReadOnlyReactiveProperty<bool> Dead { get; }

        void SetAgent(IPlayerView view, IPieceAgent agent);
    }
}
