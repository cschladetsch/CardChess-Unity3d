using System.Collections;
using App.Common.Message;
using App.Model;
using Flow;

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
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

        public Response NewGame()
        {
            return Model.NewGame();
        }

        public Response Add(ICardAgent card)
        {
            Assert.IsNotNull(card);
            return Model.Add(card.Model) ? Response.Ok : Response.Fail;
        }

        public Response<ICardAgent> DrawCard()
        {
            return null;
        }

    }
}
