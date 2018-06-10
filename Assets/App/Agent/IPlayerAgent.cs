using System;
using Flow;
using UniRx;

namespace App.Agent
{
    using Model;
    using Common;
    using Common.Message;

    public class Turnaround
    {
        public IRequest Request;
        public Action<IResponse> Responder;

        public Turnaround(IRequest request, Action<IResponse> responder)
        {
            Request = request;
            Responder = responder;
        }
    }

    /// <summary>
    /// Agent for a Player.
    /// Responsible for reacting to changes in Model state.
    /// </summary>
    public interface IPlayerAgent
        : IAgent<IPlayerModel>
        , IOwner
    {
        IDeckAgent Deck { get; }
        IHandAgent Hand { get; }
        IBoardAgent Board { get; }

        IReadOnlyReactiveProperty<int> MaxMana { get; }
        IReadOnlyReactiveProperty<int> Mana { get; }
        IReadOnlyReactiveProperty<int> Health { get; }

        ITransient StartGame();
        ITransient DrawInitialCards();
        IFuture<RejectCards> Mulligan();

        ITransient TurnStart();
        ITimedFuture<IRequest> NextRequest(float timeOut);
        ITransient TurnEnd();

        void PushRequest(Turnaround req);
    }
}
