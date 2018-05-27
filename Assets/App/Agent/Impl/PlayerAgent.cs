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

        public ITransient StartGame()
        {
            throw new NotImplementedException();
        }

        public ITransient DrawInitialCards()
        {
            throw new NotImplementedException();
        }

        IFuture<bool> IPlayerAgent.AcceptCards()
        {
            throw new NotImplementedException();
        }

        IFuture<PlacePiece> IPlayerAgent.PlaceKing()
        {
            throw new NotImplementedException();
        }

        public ITransient AcceptCards()
        {
            throw new NotImplementedException();
        }

        public ITransient PlaceKing()
        {
            throw new NotImplementedException();
        }

        public ITransient ChangeMaxMana(int i)
        {
            throw new NotImplementedException();
        }

        IFuture<ICardModel> IPlayerAgent.DrawCard()
        {
            throw new NotImplementedException();
        }

        public IFuture<PlacePiece> PlayCard()
        {
            throw new NotImplementedException();
        }

        public IFuture<MovePiece> MovePiece()
        {
            throw new NotImplementedException();
        }

        public IFuture<Pass> Pass()
        {
            throw new NotImplementedException();
        }

        public ITransient DrawCard()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

    }
}
