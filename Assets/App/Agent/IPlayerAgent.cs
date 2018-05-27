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
        ITransient StartGame();
        ITransient DrawInitialCards();
        IFuture<bool> AcceptCards();
        IFuture<PlacePiece> PlaceKing();
        ITransient ChangeMaxMana(int i);
        IFuture<ICardModel> DrawCard();
        IFuture<PlacePiece> PlayCard();
        IFuture<MovePiece> MovePiece();
        IFuture<Pass> Pass();
    }
}
