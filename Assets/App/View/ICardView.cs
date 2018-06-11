using UniRx;

namespace App.View
{
    using Agent;

    public interface ICardView
        : IView<ICardAgent>
    {
        IReadOnlyReactiveProperty<ICardView> MouseOver { get; }

        void SetAgent(IPlayerView player, ICardAgent agent);
    }
}
