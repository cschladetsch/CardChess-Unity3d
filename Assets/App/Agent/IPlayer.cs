using System;
using App.Action;
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
        IFuture<int> RollDice();
        IFuture<Action.PlayCard> PlayCard();
        IFuture<Action.MovePiece> MovePiece();
        IFuture<bool> Pass();

        IGenerator DrawInitialCards();
        void RedrawCards(params Guid[] rejected);
        ITransient HasAcceptedCards();
        void AcceptCards();

        IFuture<Action.PlayCard> HasPlacedKing();
        void PlaceKing(Coord coord);
        void AcceptKingPlacement();
    }
}
