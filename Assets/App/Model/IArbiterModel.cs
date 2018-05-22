using App.Common;

namespace App.Model
{
    public interface IArbiterModel
        : IModel
    {
        #region Properties

        IBoardModel Board { get; }
        IPlayerModel WhitePlayer { get; }
        IPlayerModel BlackPlayer { get; }
        IPlayerModel CurrentPlayer { get; }
        IPlayerModel OtherPlayer { get; }
        EGameState GameState { get; }

        #endregion

        #region Methods

        void NewGame(IPlayerModel white, IPlayerModel black);
        void Endame();

        #endregion
    }
}
