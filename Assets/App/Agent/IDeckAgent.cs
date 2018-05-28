using Flow;

namespace App.Agent
{
    using Common;

    public interface IDeckAgent
        : IAgent<Model.IDeckModel>
        //, ICardCollection<ICardAgent>
    {
        void NewGame();
        IFuture<ICardAgent> Draw();
    }
}
