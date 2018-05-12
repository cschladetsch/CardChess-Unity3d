using System.Collections;
using App.Model;
using Flow;
using UnityEngine.Assertions;

namespace App.Agent
{
    public class CardInstance : AgentBaseCoro<Model.ICardInstance>, ICardInstance
    {
        protected override IEnumerator Next(IGenerator self)
        {
            throw new System.NotImplementedException();
        }
    }
}
