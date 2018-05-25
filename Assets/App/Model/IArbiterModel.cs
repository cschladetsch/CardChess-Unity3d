using App.Common;

namespace App.Model
{
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
        Response Arbitrate(Action.IRequest request);
        void Endame();

        #endregion
    }
}
