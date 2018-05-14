using System.Collections;
using Flow;

namespace App.Agent
{
    public class CardInstance : AgentBaseCoro<Model.ICardInstance>, ICardInstance
    {
        public int Health => Model.Health;

        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
    }
}
