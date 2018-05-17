using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;

namespace App.Agent
{
    public interface IDeck :
        IAgent<Model.IDeck>,
        ICardCollection
    {
        ICard Draw();
    }
}
