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

        void SetPlayers(IPlayerModel white, IPlayerModel black);
        void NewGame();
        void Endame();

        #endregion
    }
}
