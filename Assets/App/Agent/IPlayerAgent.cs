using System;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common.Message;
    using Model;
    using Common;

    /// <summary>
    /// Agent for a Player.
    ///
    /// Responsible for reacting to changes in Model state.
    /// </summary>
    public interface IPlayerAgent
        : IAgent<IPlayerModel>
        , IOwner
    {
        #region Properties
        ICardAgent King { get; }
        IDeckAgent Deck { get; }
        IHandAgent Hand { get; }

        IReadOnlyReactiveProperty<int> MaxMana { get; }
        IReadOnlyReactiveProperty<int> Mana { get; }
        IReadOnlyReactiveProperty<int> Health { get; }
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
