using System;
using System.Collections;
using System.Linq;
using Flow;

using App.Common.Message;
using App.Model;
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
    public class PlayerAgent
        : AgentBaseCoro<Model.IPlayerModel>
            , IPlayerAgent
    {
        public EColor Color => Model.Color;
        public ICardAgent King { get; }
        public int Health => Model.Health;
        public IDeckAgent Deck { get; private set; }
        public IHandAgent Hand { get; private set; }

        public void NewGame()
        {

        }

        public ITimer StartGame()
        {
            throw new NotImplementedException();
        }

        public ITimer DrawInitialCards()
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
