using System.Collections;
using System.Collections.Generic;

using Flow;
using UniRx;

namespace App.Agent
{
    using Common;
    using Common.Message;
    using Model;

    /// <inheritdoc cref="IPlayerAgent" />
    /// <summary>
    /// The agent that represents a playerAgent in the game.
    /// </summary>
    public abstract class PlayerAgentBase
        : AgentBaseCoro<IPlayerModel>
        , IPlayerAgent
    {
        public EColor Color => Model.Color;
        public ICardAgent King { get; private set; }
        public IPieceAgent KingPiece { get; set; }
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

        public override void Create()
        {
            base.Create();
            Assert.IsNotNull(Model);
            Assert.IsNotNull(Model.King);
            Assert.IsNotNull(Model.Deck);
            Assert.IsNotNull(Model.Hand);

            King = Registry.New<ICardAgent>(Model.King);
            Deck = Registry.New<IDeckAgent>(Model.Deck);
            Hand = Registry.New<IHandAgent>(Model.Hand);

            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);
        }

        public virtual ITransient StartGame()
        {
            Verbose(60, $"{Model} StartGame");
            Model.StartGame();
            return null;
        }

        public ITransient DrawInitialCards()
        {
            Verbose(60, $"{Model} Draws first cards");
            Model.DrawHand();
            return null;
        }

        public abstract IFuture<RejectCards> Mulligan();
        public abstract IFuture<PlacePiece> PlaceKing();
        public abstract ITransient TurnStart();
        public abstract ITimedFuture<IRequest> NextRequest(float seconds);
        public abstract ITransient TurnEnd();

        public void PushRequest(Turnaround req)
        {
            _Requests.Add(req);
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

        protected readonly List<Turnaround> _Requests = new List<Turnaround>();
    }
}
