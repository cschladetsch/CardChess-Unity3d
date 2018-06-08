using UniRx;

namespace App.View
{
    using Agent;

    public interface IHandView
        : IView<IHandAgent>
    {
        //IReactiveProperty<ICardAgent> Hover { get; }
    }
}
