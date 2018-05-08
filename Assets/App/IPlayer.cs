using Flow;

namespace App
{
    /// <summary>
    /// A Player in the game.
    /// Hopefully, these could be bots, or remote players as well
    /// as simple hotseat players at the same local device.
    /// </summary>
    public interface IPlayer
    {
        EColor Color { get; }
        Flow.IFuture<int> RollDice();
        void AddMaxMana(int mana);
        IFuture<PlayCard> TryPlayCard();
        IFuture<MovePiece> TryMovePiece();
        IFuture<bool> Pass();
    }
}