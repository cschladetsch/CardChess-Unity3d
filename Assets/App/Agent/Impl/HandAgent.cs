using System.Collections;
using App.Model;
using Flow;

namespace App.Agent
{
    using Common;

    public class HandAgent :
        AgentBaseCoro<Model.IHandModel>,
        IHandAgent
    {
        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

        public void Add(ICardAgent card)
        {
            Model.Add(card.Model);
        }
    }
}
