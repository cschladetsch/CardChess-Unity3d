using Dekuple.Agent;
using UniRx;

namespace App.View
{
    using Common;
    using Dekuple;
    using Dekuple.View;
    using Agent;

    /// <summary>
    /// View of the arbitrator of the game
    /// </summary>
    public interface IArbiterView
        : IView<IArbiterAgent>
    {
        IBoardView BoardView { get; }
        // IPlayerView CurrentPlayerView { get; }
        // EColor CurrentPlayerColor { get; }
        IPlayerView WhitePlayerView { get; }
        IPlayerView BlackPlayerView { get; }

        bool CurrentPlayerOwns(IOwned owned);
        IPlayerView GetPlayerView(IAgent agent);
    }
}
