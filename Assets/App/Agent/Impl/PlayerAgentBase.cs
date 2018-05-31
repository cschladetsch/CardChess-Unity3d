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
        public ICardAgent King { get; private set; }
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

        public virtual ITransient NewGame()
        {
            Deck = Registry.New<IDeckAgent>(Model.Deck);
            Hand = Registry.New<IHandAgent>(Model.Hand);
            King = Registry.New<ICardAgent>(Model.King);
            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);

            Deck.NewGame();
            Hand.NewGame();

            return null;
        }

        public ITransient DrawInitialCards()
        {
            Model.DrawHand();
            foreach (var card in Model.Hand.Cards)
            {
            }
            return null;
        }

        public abstract ITransient StartGame();
        public abstract IFuture<List<ICardModel>> Mulligan();
        public abstract IFuture<MovePiece> PlaceKing();
        public abstract ITransient TurnStart();
        public abstract IFuture<IRequest> NextRequest();
        public abstract ITransient TurnEnd();

        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
    }
}
