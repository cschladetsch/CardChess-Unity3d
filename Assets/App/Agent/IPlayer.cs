using App.Model;
using Flow;

namespace App.Agent
{
    public interface IPlayer : IAgent<Model.IPlayer>
    {
        EColor Color { get; }
        ICardInstance King { get; }
        int Health { get; }

        IFuture<EResponse> ChangeMaxMana(int mana);
        IFuture<EResponse> ChangeMana(int mana);
        IFuture<EResponse> NewGame();
        ITransient StartGame();
        IFuture<EResponse> DrawInitialCards();
        IFuture<int> RollDice();
        IFuture<Action.PlayCard> PlaceKing();
        IFuture<Action.PlayCard> PlayCard();
        IFuture<Action.MovePiece> MovePiece();
        IFuture<bool> Pass();
        ITransient Mulligan();
    }
}
