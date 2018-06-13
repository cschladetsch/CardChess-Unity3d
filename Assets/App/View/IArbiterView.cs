using App.Common;

namespace App.View
{
    using Agent;

    public interface IArbiterView
        : IView<IArbiterAgent>
    {
        IBoardView BoardView { get; }
        IPlayerView WhitePlayerView { get; }
        IPlayerView BlackPlayerView { get; }
        EColor CurrentPlayerColor { get; }

        bool CurrentPlayerOwns(IOwned owned);
    }
}
