using App.Model;

namespace App.View
{
    using Dekuple.View;
    using UniRx;
    using Agent;

    /// <summary>
    /// A view of a card that is not on the board.
    /// </summary>
    public interface ICardView
        : IView<ICardAgent>
    {
        IPlayerModel PlayerModel { get; }
        IReadOnlyReactiveProperty<ICardView> MouseOver { get; }
    }
}
