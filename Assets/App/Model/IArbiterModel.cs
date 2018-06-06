using App.Common.Message;
using JetBrains.Annotations;
using UniRx;

namespace App.Model
{
    /// <summary>
    /// The referee for the game.
    /// </summary>
    public interface IArbiterModel
        : IModel
    {
        IBoardModel Board { get; }
        IReadOnlyReactiveProperty<IPlayerModel> CurrentPlayer { get; }
        IReadOnlyReactiveProperty<EGameState> GameState { get; }

        void PrepareGame(IPlayerModel white, IPlayerModel black);
        Response Arbitrate(IRequest request);
    }
}
