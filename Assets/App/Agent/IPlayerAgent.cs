using System;
using System.Collections.Generic;
using Flow;
using UniRx;

namespace App.Agent
{
    using Model;
    using Common;
    using Common.Message;

    /// <summary>
    /// Agent for a Player.
    /// Responsible for reacting to changes in Model state.
    /// </summary>
    public interface IPlayerAgent
        : IAgent<IPlayerModel>
        , IOwner
    {
        ICardAgent King { get; }
        IDeckAgent Deck { get; }
        IHandAgent Hand { get; }
        IReadOnlyReactiveProperty<int> MaxMana { get; }
        IReadOnlyReactiveProperty<int> Mana { get; }
        IReadOnlyReactiveProperty<int> Health { get; }

        ITransient StartGame();
        ITransient DrawInitialCards();
        IFuture<List<ICardModel>> Mulligan();
        IFuture<MovePiece> PlaceKing();

        ITransient TurnStart();
        IFuture<IRequest> NextRequest();
        ITransient TurnEnd();
    }
}
