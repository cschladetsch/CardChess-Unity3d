using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flow;

namespace App.Agent
{
    public interface IBoard : IAgent<Model.IBoard>
    {
        IFuture<bool> NewGame();
    }
}
