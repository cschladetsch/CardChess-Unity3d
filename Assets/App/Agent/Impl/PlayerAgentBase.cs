using System;
using System.Collections;
using System.Collections.Generic;

using Flow;
using UniRx;

namespace App.Agent
{
    using Common;
    using Common.Message;
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
            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);

            Deck.StartGame();
            Hand.StartGame();
        }

        public override string ToString()
        {
            return $"Agent for {Model}";
        }

        public virtual ITimedFuture<IRequest> NextRequest(float seconds)
        {
            var req = Model.NextAction();
            return New.TimedFuture(TimeSpan.FromSeconds(seconds), req);
        }

        public void PushRequest(Turnaround req)
        {
            _Requests.Add(req);
        }

        protected readonly List<Turnaround> _Requests = new List<Turnaround>();
    }
}
