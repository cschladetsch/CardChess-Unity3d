using System;
using App.Action;
using Flow;

namespace App.Agent
{
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// Agent for a Player. Responsible for change over time.
    /// </summary>
    public interface IPlayer :
        IAgent<Model.IPlayer>,
        IOwner
    {
        #region Properties
        EColor Color { get; }
        ICardInstance King { get; }
        int Health { get; }
        PlayerDeckCollection Deck { get; }
        PlayerHandCollection Hand { get; }
        //ModelDeckCollection ModelDeck { get; }
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
        IFuture<ICardInstance> FutureDrawCard();
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
