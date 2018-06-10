using System.Collections;

using Flow;

namespace App.Agent
{
    using Common;
    using Common.Message;
    using Model;

    public class HandAgent
        : AgentBaseCoro<IHandModel>
        , IHandAgent
    {
        public HandAgent(IHandModel model)
            : base(model)
        {
            //model.Cards.ObserveAdd().Subscribe(async
        }

        //protected override IEnumerator Next(IGenerator self)
        //{
        //    yield return null;
        //}

        public IFuture<Response> NewGame()
        {
            Model.NewGame();
            return null;
        }

        public IFuture<Response> Add(ICardAgent card)
        {
            Assert.IsNotNull(card);
            return New.Future(Model.Add(card.Model) ? Response.Ok : Response.Fail);
        }

        public IFuture<Response> Remove(ICardAgent model)
        {
            return New.Future(Response.Ok);
        }
    }
}
