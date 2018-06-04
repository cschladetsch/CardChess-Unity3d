using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Agent;

namespace Assets.App.Agent
{
    interface IAgentCardCollection
    {
        ICardAgent Get(Guid id);
    }
}
