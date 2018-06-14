using App.Common.Message;
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
        IReactiveProperty<int> TurnNumber { get; }
        IReadOnlyReactiveProperty<EGameState> GameState { get; }
        IReadOnlyReactiveProperty<IPlayerModel> CurrentPlayer { get; }

        void PrepareGame(IPlayerModel white, IPlayerModel black);
        IResponse Arbitrate(IRequest request);
        void EndTurn();
    }
}
