using System;
using Flow;

namespace App.Agent
{
    /// <inheritdoc />
    /// <summary>
    /// Agent for a Player. Responsible for change over time.
    /// </summary>
    public interface IPlayer : IAgent<Model.IPlayer>
    {
        #region Properties
        EColor Color { get; }
        ICardInstance King { get; }
        int Health { get; }
        PlayerDeckCollection Deck { get; }
        PlayerHandCollection Hand { get; }
        #endregion

        #region Methods
        void AcceptCards();
        void RedrawCards(params Guid[] rejected);
        void PlaceKing(Action.Coord coord);
        void AcceptKingPlacement();

        #region Flow Methods
        IFuture<EResponse> NewGame();
        ITransient StartGame();
        IFuture<EResponse> ChangeMaxMana(int mana);
        IFuture<EResponse> ChangeMana(int mana);
        IFuture<int> RollDice();
        IFuture<Action.PlayCard> PlayCard();
        IFuture<Action.MovePiece> MovePiece();
        IFuture<bool> Pass();
        IGenerator DrawInitialCards();
        IFuture<bool> HasAcceptedCards();
        IFuture<Action.PlayCard> HasPlacedKing();
        ITransient DeliverCards();
        ITransient DrawCard();

        #endregion
        #endregion
    }
}
