namespace App.Agent
{
    using System;
    using Flow;
    using UniRx;
    using Dekuple;
    using Dekuple.Agent;
    using Model;
    using Common;
    using Common.Message;

    /// <inheritdoc cref="IAgent{TModel}" />
    /// <summary>
    /// Agent for a Player.
    /// </summary>
    public interface IPlayerAgent
        : IGameAgent<IPlayerModel>
        , IOwner
    {
        EColor Color { get; }
        IDeckAgent Deck { get; }
        IHandAgent Hand { get; }
        IBoardAgent Board { get; }
        IEndTurnButtonAgent EndTurnButton { get; }

        IReadOnlyReactiveProperty<int> MaxMana { get; }
        IReadOnlyReactiveProperty<int> Mana { get; }
        IReadOnlyReactiveProperty<int> Health { get; }

        IFuture<RejectCards> Mulligan();
        ITransient TurnStart();
        ITimedFuture<Turnaround> NextRequest(float timeOut);
        ITransient TurnEnd();

        /// <summary>
        /// Push a request onto the action stack of the <see cref="IArbiterAgent" />.
        ///
        /// TODO: This should probably return a Flow.IFuture&lt;IResponse&gt;
        /// </summary>
        /// <param name="request"></param>
        /// <param name="handler"></param>
        void PushRequest(IRequest request, Action<IResponse> handler);
    }
}
