using Flow;

namespace App.Agent
{
    using Common;

    public interface IDeckAgent
        : IAgent<Model.IDeckModel>
    {
        void NewGame();
        IFuture<ICardAgent> Draw();
    }
}
