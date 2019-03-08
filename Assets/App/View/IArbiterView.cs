using App.Common;
using Dekuple;
using Dekuple.View;

namespace App.View
{
    using Agent;

    /// <summary>
    /// View of the arbitrator of the game
    /// </summary>
    public interface IArbiterView
        : IView<IArbiterAgent>
    {
        IBoardView BoardView { get; }
        IPlayerView CurrentPlayerView { get; }
        EColor CurrentPlayerColor { get; }

        bool CurrentPlayerOwns(IOwned owned);
    }
}
