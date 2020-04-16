namespace App.Agent
{
    using System;
    using System.Collections.Generic;
    using UniRx;
    using Flow;
    using Dekuple;
    using Dekuple.Agent;
    using Common;
    using Common.Message;
    using Model;

    /// <inheritdoc cref="AgentBaseCoro{TModel}" />
    /// <summary>
    /// The agent that represents a player in the game.
    /// </summary>
    public abstract class PlayerAgentBase
        : AgentBaseCoro<IPlayerModel>
        , IPlayerAgent
    {
        [Inject] public IBoardAgent Board { get; set; }

        public EColor Color => Model.Color;
        public IDeckAgent Deck { get; set; }
        public IHandAgent Hand { get; set; }
        public IEndTurnButtonAgent EndTurnButton { get; set;}

        public IReadOnlyReactiveProperty<int> MaxMana => Model.MaxMana;
        public IReadOnlyReactiveProperty<int> Mana => Model.Mana;
        public IReadOnlyReactiveProperty<int> Health => Model.Health;
        public IReadOnlyReactiveProperty<bool> Dead { get; private set; }

        protected readonly List<Turnaround> _Requests = new List<Turnaround>();
        protected readonly List<IFuture<Turnaround>> _Futures = new List<IFuture<Turnaround>>();

        protected PlayerAgentBase(IPlayerModel model)
            : base(model)
        {
        }

        public abstract IFuture<RejectCards> Mulligan();
        public abstract ITransient TurnStart();
        public abstract ITransient TurnEnd();

        public IPlayerModel TypedModel => Model as IPlayerModel;

        public virtual void StartGame()
        {
            Assert.IsNotNull(Model);
            Assert.IsNotNull(Model.Deck);
            Assert.IsNotNull(Model.Hand);

            Deck = Registry.Get<IDeckAgent>(Model.Deck);
            Hand = Registry.Get<IHandAgent>(Model.Hand);
            EndTurnButton = Registry.Get<IEndTurnButtonAgent>(Model.EndTurnButton);

            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);

            Deck.StartGame();
            Hand.StartGame();
        }

        public virtual void EndGame()
        {
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
            Info($"{response.Request} => {response.Type} : {response.Error}");
        }

        public override string ToString()
        {
            return $"Agent for {Model}";
        }
    }
}
