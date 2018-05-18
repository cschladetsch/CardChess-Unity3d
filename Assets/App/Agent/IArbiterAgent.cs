using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Agent;
using App.Model;
using Flow;

namespace App.Agent
{
    class IArbiterAgent :
        AgentBaseCoro<Model.IArbiterModel>
    {
        protected override IEnumerator Next(IGenerator self)
        {
            yield return null;
        }
    }
}
