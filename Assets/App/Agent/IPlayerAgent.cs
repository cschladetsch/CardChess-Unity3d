using System;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Agent
{
    using Common;
    using Registry;

    /// <summary>
    /// Agent for a PlayerAgent. Responsible for reacting to changes in Model state.
    /// </summary>
    public interface IPlayerAgent :
        IAgent<Model.IPlayerModel>,
        IOwner
    {
        #region Properties
        ICardAgent King { get; }
        int Health { get; }
        IDeckAgent Deck { get; }
        IHandAgent Hand { get; }
        #endregion

        void NewGame();
        ITimer StartGame();
        ITimer DrawInitialCards();
        ITimedFuture<bool> AcceptCards();
        ITimedFuture<PlacePiece> PlaceKing();
        ITransient ChangeMaxMana(int i);
        ITimedFuture<ICardModel> DrawCard();
        ITimedFuture<PlacePiece> PlayCard();
        ITimedFuture<MovePiece> MovePiece();
        ITimedFuture<Pass> Pass();
    }
}
