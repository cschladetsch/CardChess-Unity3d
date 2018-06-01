using System;
using System.Collections;
using System.Collections.Generic;
using Flow;

using App.Common.Message;
using App.Model;
using App.Registry;
using UniRx;

namespace App.Agent
{
    using Common;

    /// <inheritdoc cref="IPlayerAgent" />
    /// <summary>
    /// The agent that represents a playerAgent in the game.
    /// </summary>
    public abstract class PlayerAgentBase
        : AgentBaseCoro<IPlayerModel>
        , IPlayerAgent
    {
        public EColor Color => Model.Color;
        public ICardModel King { get; private set; }
        public IDeckModel Deck { get; set; }
        public IHandModel Hand { get; set; }

        public IReadOnlyReactiveProperty<int> MaxMana => Model.MaxMana;
        public IReadOnlyReactiveProperty<int> Mana => Model.Mana;
        public IReadOnlyReactiveProperty<int> Health => Model.Health;
        public ReactiveProperty<bool> Dead { get; private set; }

        protected PlayerAgentBase(IPlayerModel model)
            : base(model)
        {
        }

        public virtual ITransient StartGame()
        {
            Info($"{this} StartGame");
            Model.NewGame();
            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);
            return null;
        }

        public ITransient DrawInitialCards()
        {
            Info($"{this} Draws first cards");
            Model.DrawHand();
            return null;
        }

        public abstract IFuture<RejectCards> Mulligan();
        public abstract IFuture<PlacePiece> PlaceKing();
        public abstract ITransient TurnStart();
        public abstract IFuture<IRequest> NextRequest();
        public abstract ITransient TurnEnd();

        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
    }
}
