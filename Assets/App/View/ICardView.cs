using UniRx;

namespace App.View
{
    using Agent;

    public interface ICardView
        : IView<ICardAgent>
        //, ICard
    {
        IReadOnlyReactiveProperty<ICardView> MouseOver { get; }

        void SetAgent(IPlayerView player, ICardAgent agent);
    }
}
