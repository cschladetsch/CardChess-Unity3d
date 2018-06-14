using UniRx;

namespace App.View
{
    using Agent;
    using Common;

    public interface IPieceView
        : IView<IPieceAgent>
        //, ICard
    {
        //IReadOnlyReactiveProperty<IPieceView> MouseOver { get; }
        IReactiveProperty<Coord> Coord { get; }

        void SetAgent(IPlayerView view, IPieceAgent agent);
    }
}
