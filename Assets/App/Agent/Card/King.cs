using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common;
using Flow;

namespace App.Agent.Card
{
    public class AgentBaseCardCoro<TCardModel>
        : AgentBaseCoro<Model.ICardModel>
        , ICardAgent
    {
        public string Description { get; }
        public int Attack { get; }
        public int Health { get; }
        public IEnumerable<IEffect> Effects { get; }
        public ECardType Type { get; }
        public EPieceType PieceType { get; }

        protected override IEnumerator Next(IGenerator self)
        {
            throw new NotImplementedException();
        }

        public bool SameOwner(IOwned other)
        {
            return Owner == other?.Owner;
        }
    }
}
