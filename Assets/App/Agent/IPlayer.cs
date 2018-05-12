using App.Model;
using Flow;

namespace App.Agent
{
    public interface IPlayer : IAgent<Model.IPlayer>
    {
        EColor Color { get; }

        IFuture<EResponse> AddMaxMana(int mana);
        IFuture<EResponse> AddMana(int mana);
        IFuture<int> RollDice();
        IFuture<Action.PlayCard> PlaceKing();
        IFuture<Action.PlayCard> PlayCard();
        IFuture<Action.MovePiece> MovePiece();
        IFuture<bool> Pass();
        IFuture<EResponse> NewGame();
    }
}
