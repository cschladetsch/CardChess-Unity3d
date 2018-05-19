namespace App.Model
{
    public interface IArbiterModel
        : IModel
    {
        IBoardModel Board { get; }
        IPlayerModel WhitePlayer { get; }
        IPlayerModel BlackPlayer { get; }

        void NewGame();
        void Endame();

        void PrepareDecks();
        void DrawCards();
        void PlayerTurn();
    }
}
