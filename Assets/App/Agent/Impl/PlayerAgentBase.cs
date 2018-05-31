using System;
using System.Collections;
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
    public class PlayerAgentBase
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

        public PlayerAgentBase(IPlayerModel model)
            : base(model)
        {
        }

        public virtual void NewGame()
        {
            Deck.NewGame();
            Hand.NewGame();
            King = Registry.New<ICardAgent>(Model.King);
            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);
        }

        public ITimer StartGame()
        {
            return null;
        }

        public ITransient DrawInitialCards()
        {
            return null;
        }

        public ITimedFuture<bool> AcceptCards()
        {
            return null;
        }

        public ITimedFuture<PlacePiece> PlaceKing()
        {
            return null;
        }

        public ITransient ChangeMaxMana(int i)
        {
            return null;
        }

        public ITimedFuture<ICardModel> DrawCard()
        {
            return null;
        }

        public ITimedFuture<PlacePiece> PlayCard()
        {
            return null;
        }

        public ITimedFuture<MovePiece> MovePiece()
        {
            return null;
        }

        public ITimedFuture<Pass> Pass()
        {
            return null;
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
    }
}
