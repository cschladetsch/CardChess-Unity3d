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
        #region Properties
        IBoardModel Board { get; }
        IReadOnlyReactiveProperty<IPlayerModel> CurrentPlayer { get; }
        IReadOnlyReactiveProperty<EGameState> GameState { get; }
        #endregion

        #region Methods
        void NewGame(IPlayerModel white, IPlayerModel black);
        Response Arbitrate(IRequest request);
        #endregion
    }
}
