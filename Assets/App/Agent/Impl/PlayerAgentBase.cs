using System;
using System.Collections.Generic;

using Flow;
using UniRx;

namespace App.Agent
{
    using Common;
    using Dekuple.Common;
    using Common.Message;
    using Dekuple.Common.Message;
    using Model;

    /// <summary>
    /// The agent that represents a player in the game.
    /// </summary>
    public abstract class PlayerAgentBase
        : AgentBaseCoro<IPlayerModel>
        , IPlayerAgent
    {
        public EColor Color => Model.Color;
        public IDeckAgent Deck { get; set; }
        public IHandAgent Hand { get; set; }
        public IEndTurnButtonAgent EndTurnButton { get; set;}

        public IReadOnlyReactiveProperty<int> MaxMana => Model.MaxMana;
        public IReadOnlyReactiveProperty<int> Mana => Model.Mana;
        public IReadOnlyReactiveProperty<int> Health => Model.Health;
        public ReactiveProperty<bool> Dead { get; private set; }

        protected PlayerAgentBase(IPlayerModel model)
            : base(model)
        {
        }

        public abstract IFuture<RejectCards> Mulligan();
        public abstract ITransient TurnStart();
        public abstract ITransient TurnEnd();

        public override void StartGame()
        {
            base.StartGame();

            Assert.IsNotNull(Model);
            Assert.IsNotNull(Model.Deck);
            Assert.IsNotNull(Model.Hand);

            Deck = Registry.New<IDeckAgent>(Model.Deck);
            Hand = Registry.New<IHandAgent>(Model.Hand);
            EndTurnButton = Registry.New<IEndTurnButtonAgent>(Model.EndTurnButton);

            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);

            Deck.StartGame();
            Hand.StartGame();
        }

        public void PushRequest(IRequest request, Action<IResponse> handler)
        {
            _Requests.Add(new Turnaround(request, handler));
        }

        public virtual ITimedFuture<Turnaround> NextRequest(float seconds)
        {
            var future = New.TimedFuture<Turnaround>(TimeSpan.FromSeconds(seconds));
            future.Name = "Request for " + Name;
            _Futures.Add(future);
            future.TimedOut += f => _Futures.RemoveRef(future);
            return future;
        }

        protected void ResponseHandler(IResponse response)
        {
            Info($"{response.Request} => {response.Type}:{response.Error}");
        }

        public override string ToString()
        {
            return $"Agent for {Model}";
        }

        protected readonly List<Turnaround> _Requests = new List<Turnaround>();
        protected readonly List<IFuture<Turnaround>> _Futures = new List<IFuture<Turnaround>>();
    }
}
