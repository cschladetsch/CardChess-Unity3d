using App.Common;
using App.Common.Message;
using UniRx;

namespace App.Model
{
    /// <summary>
    /// The referee for the game.
    /// </summary>
    public interface IArbiterModel
        : IModel
        , IGameActor
    {
        IBoardModel Board { get; }
        IReadOnlyReactiveProperty<IPlayerModel> CurrentPlayer { get; }
        IReadOnlyReactiveProperty<EGameState> GameState { get; }
        IReactiveProperty<int> TurnNumber { get; }

        void PrepareGame(IPlayerModel white, IPlayerModel black);
        Response Arbitrate(IRequest request);
        void EndTurn();
    }
}
