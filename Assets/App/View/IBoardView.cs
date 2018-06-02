using UniRx;

namespace App.View
{
    using Agent;

    public interface IBoardView
        : IView<IBoardAgent>
    {
        IReadOnlyReactiveProperty<ISquareView> CurrentSquare { get; }
    }
}
