using System;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common.Message;
    using Model;
    using Common;

    /// <summary>
    /// Agent for a PlayerAgent. Responsible for reacting to changes in Model state.
    /// </summary>
    public interface IPlayerAgent
        : IAgent<IPlayerModel>
        , IOwner
    {
        #region Properties
        ICardAgent King { get; }
        IDeckAgent Deck { get; }
        IHandAgent Hand { get; }

        ReactiveProperty<int> Health { get; }
        ReactiveProperty<int> MaxMana { get; }
        ReactiveProperty<int> Mana { get; }
        #endregion

        void NewGame();
        ITimer StartGame();
        ITransient DrawInitialCards();
        ITimedFuture<bool> AcceptCards();
        ITimedFuture<PlacePiece> PlaceKing();
        ITransient ChangeMaxMana(int i);
        ITimedFuture<ICardModel> DrawCard();
        ITimedFuture<PlacePiece> PlayCard();
        ITimedFuture<MovePiece> MovePiece();
        ITimedFuture<Pass> Pass();
    }
}
