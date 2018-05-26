using App.Common.Message;

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
        IPlayerModel CurrentPlayer { get; }
        EGameState GameState { get; }

        #endregion

        #region Methods

        void NewGame(IPlayerModel white, IPlayerModel black);
        Response Arbitrate(IRequest request);
        void Endame();

        #endregion
    }
}
