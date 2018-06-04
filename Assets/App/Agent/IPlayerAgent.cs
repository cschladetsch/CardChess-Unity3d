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
        IPieceAgent KingPiece { get; set; }
        IDeckAgent Deck { get; }
        IHandAgent Hand { get; }

        IReadOnlyReactiveProperty<int> MaxMana { get; }
        IReadOnlyReactiveProperty<int> Mana { get; }
        IReadOnlyReactiveProperty<int> Health { get; }

        ITransient StartGame();
        ITransient DrawInitialCards();
        IFuture<RejectCards> Mulligan();
        IFuture<PlacePiece> PlaceKing();

        ITransient TurnStart();
        ITimedFuture<IRequest> NextRequest(float timeOut);
        ITransient TurnEnd();
    }
}
