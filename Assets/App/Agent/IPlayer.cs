using App.Model;
using Flow;

namespace App.Agent
{
    public interface IPlayer
    {
        EColor Color { get; }

        IFuture<EResponse> AddMaxMana(int mana);
        IFuture<EResponse> AddMana(int mana);
        IFuture<int> RollDice();
        IFuture<PlayCard> PlaceKing();
        IFuture<PlayCard> PlayCard();
        IFuture<MovePiece> MovePiece();
        IFuture<bool> Pass();
        IFuture<EResponse> NewGame();
    }
}
