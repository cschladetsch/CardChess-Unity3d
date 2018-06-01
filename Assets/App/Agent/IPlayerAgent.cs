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
        ICardModel King { get; }
        IDeckModel Deck { get; }
        IHandModel Hand { get; }

        // TODO: will we ever need CardAgents? Why?
        // We will need them when they have their own behavior,
        // like idle/attack animations etc. For now, we'll use
        // static models.
        //ICardAgent King { get; }
        //IDeckAgent Deck { get; }
        //IHandAgent Hand { get; }
        IReadOnlyReactiveProperty<int> MaxMana { get; }
        IReadOnlyReactiveProperty<int> Mana { get; }
        IReadOnlyReactiveProperty<int> Health { get; }

        ITransient StartGame();
        ITransient DrawInitialCards();
        IFuture<RejectCards> Mulligan();
        IFuture<PlacePiece> PlaceKing();

        ITransient TurnStart();
        IFuture<IRequest> NextRequest();
        ITransient TurnEnd();
    }
}
