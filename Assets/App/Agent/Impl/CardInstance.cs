using System;
using System.Collections;
using System.Collections.Generic;
using App.Common;
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
        public string Description { get; }
        public int Attack { get; }
        public int Health => Model.Health;
        public IList<IEffect> Effects { get; }
        public ECardType Type { get; }
        public bool SameOwner(IOwner other)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }

        public IOwner Owner { get; }
    }
}
