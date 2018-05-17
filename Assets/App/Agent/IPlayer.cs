using System;
using App.Action;
using Flow;

namespace App.Agent
{
    using Common;

    /// <summary>
    /// Agent for a Player. Responsible for reacting to changes in Model state.
    /// </summary>
    public interface IPlayer :
        IAgent<Model.IPlayer>,
        IOwner
    {
        #region Properties
        ICard King { get; }
        int Health { get; }
        IDeck Deck { get; }
        IHand Hand { get; }
        #endregion

        #region Methods

        #region State
        IFuture<EResponse> NewGame();
        IFuture<int> RollDice();
        ITransient StartGame();
        IGenerator DrawInitialCards();
        IFuture<EResponse> ChangeMaxMana(int delta);
        IFuture<EResponse> ChangeMana(int delta);
        #endregion

        #region Turn Options
        IFuture<ICard> FutureDrawCard();
        IFuture<PlayCard> FuturePlaceKing();
        IFuture<bool> FutureAcceptCards();
        IFuture<PlayCard> FuturePlayCard();
        IFuture<MovePiece> FutureMovePiece();
        IFuture<bool> FuturePass();
        #endregion

        #region Command Methods
        void RedrawCards(params Guid[] rejected);
        void AcceptCards();
        void PlaceKing(Coord coord);
        void PlayCard(PlayCard playCard);
        void MovePiece(MovePiece move);
        void Pass();
        #endregion

        #endregion
    }
}
