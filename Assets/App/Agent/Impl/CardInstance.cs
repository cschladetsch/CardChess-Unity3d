using System;
using System.Collections;
using Flow;

namespace App.Agent
{
    /// <summary>
    /// An Instance in Hand or in the Deck
    /// </summary>
    public class CardInstance :
        AgentBaseCoro<Model.ICardInstance>,
        ICardInstance
    {
        public int Health => Model.Health;

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }
    }
}
