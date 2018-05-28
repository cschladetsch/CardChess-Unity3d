using System;
using System.Collections;
using System.Linq;
using Flow;

using App.Common.Message;
using App.Model;
using App.Registry;
using UniRx;
#if VS
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using UnityEngine.Assertions;
#endif

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

        public ReactiveProperty<int> MaxMana { get; private set; }
        public ReactiveProperty<int> Mana { get; private set; }
        public ReactiveProperty<int> Health { get; private set; }
        public ReactiveProperty<bool> Dead { get; private set; }

        [Inject] private Service.ICardTemplateService _cardTemplateService;

        public virtual void NewGame()
        {
            Deck.NewGame();
            Hand.NewGame();
            var kingModel = _cardTemplateService.NewCardModel(Model, EPieceType.King);
            King = Registry.New<ICardAgent>(kingModel);

            MaxMana = new ReactiveProperty<int>(Model.MaxMana);
            Mana = new ReactiveProperty<int>(Model.Mana);
            Health = new ReactiveProperty<int>(Model.Health);
            Dead = Health.Select(x => x <= 0).ToReactiveProperty(false);
        }

        public ITimer StartGameTimer { get; private set; }
        public ITimer StartGame()
        {
            return StartGameTimer = Factory.OneShotTimer(TimeSpan.FromSeconds(20));
        }

        public ITransient DrawInitialCards()
        {
            throw new NotImplementedException();
        }

        public ITimedFuture<bool> AcceptCards()
        {
            throw new NotImplementedException();
        }

        public ITimedFuture<PlacePiece> PlaceKing()
        {
            throw new NotImplementedException();
        }

        public ITransient ChangeMaxMana(int i)
        {
            throw new NotImplementedException();
        }

        public ITimedFuture<ICardModel> DrawCard()
        {
            throw new NotImplementedException();
        }

        public ITimedFuture<PlacePiece> PlayCard()
        {
            throw new NotImplementedException();
        }

        public ITimedFuture<MovePiece> MovePiece()
        {
            throw new NotImplementedException();
        }

        public ITimedFuture<Pass> Pass()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerator Next(IGenerator self)
        {
            throw new NotImplementedException();
        }
    }
}
