using UniRx;

namespace App.View
{
    using Agent;

    public interface ICardView
        : IView<ICardAgent>
    {
        //IReadOnlyReactiveProperty<ICardAgent> MouseOver { get; }
        //IReadOnlyReactiveProperty<ICardAgent> Drop { get; }
    }
}
