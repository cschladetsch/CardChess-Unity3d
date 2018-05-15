using System;
using App.Action;
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
        #region Initialise
        IFuture<EResponse> NewGame();
        IFuture<int> RollDice();
        ITransient StartGame();
        IFuture<PlayCard> PlaceKing();
        IGenerator DrawInitialCards();
        ITransient AcceptCards();
        IFuture<EResponse> ChangeMaxMana(int delta);
        IFuture<EResponse> ChangeMana(int delta);
        #endregion

        #region Turn Options
        ITransient DrawCard();
        IFuture<PlayCard> PlayCard();
        IFuture<MovePiece> MovePiece();
        IFuture<bool> Pass();
        #endregion

        #region Command Methods
        void RedrawCards(params Guid[] rejected);
        void KingPlaced(Coord coord);
        void CardPlayed(PlayCard playCard);
        void PieceMoved(MovePiece move);
        void Passed();

        #endregion
        #endregion
    }
}
