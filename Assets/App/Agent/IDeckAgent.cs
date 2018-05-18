using System.Collections.Generic;
using Flow;

namespace App.Agent
{
    public interface IDeckAgent :
        IAgent<Model.IDeckModel>
    {
        int NumCards { get; }
        IEnumerable<ICardAgent> Cards { get; }

        IFuture<ICardAgent> Draw();
        void Remove(ICardAgent card);
    }
}
