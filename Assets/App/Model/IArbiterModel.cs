namespace App.Model
{
    public interface IArbiterModel
        : IModel
    {
        #region Properties
        IBoardModel Board { get; }
        IPlayerModel WhitePlayer { get; }
        IPlayerModel BlackPlayer { get; }
        #endregion

        #region Methods
        void SetPlayers(IPlayerModel white, IPlayerModel black);
        void NewGame();
        void Endame();

        void PrepareDecks();
        void DrawCards();
        void PlayerTurn();
        #endregion
    }
}
