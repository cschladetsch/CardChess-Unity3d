using Dekuple.View;
using UniRx;

namespace App.View
{
    using Agent;

    /// <summary>
    /// A view of a card that is not on the board.
    /// </summary>
    public interface ICardView
        : IView<ICardAgent>
    {
        IReadOnlyReactiveProperty<ICardView> MouseOver { get; }

        //void SetAgent(IPlayerView player, ICardAgent agent);
    }
}
