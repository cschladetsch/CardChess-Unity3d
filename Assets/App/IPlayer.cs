using Flow;

namespace App
{
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