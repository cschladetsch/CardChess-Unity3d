using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Agent
{
    public interface IHandAgent :
        IAgent<Model.IHandModel>
    {
        void NewGame();
        void Add(ICardAgent card);
    }
}
