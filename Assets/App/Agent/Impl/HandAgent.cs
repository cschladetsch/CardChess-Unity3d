using System.Collections;
using App.Common.Message;
using App.Model;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common;

    public class HandAgent
        : AgentBaseCoro<Model.IHandModel>
        , IHandAgent
    {
        public HandAgent(IHandModel model)
            : base(model)
        {
            //model.Cards.ObserveAdd().Subscribe(async
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

        public IFuture<Response> NewGame()
        {
            return New.Future(Model.NewGame());
        }

        public IFuture<Response> Add(ICardAgent card)
        {
            Assert.IsNotNull(card);
            return New.Future(Model.Add(card.Model) ? Response.Ok : Response.Fail);
        }

        public IFuture<Response> Remove(ICardAgent agent)
        {
            return New.Future(Response.Ok);
        }
    }
}
