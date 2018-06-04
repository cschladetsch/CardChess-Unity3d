using Flow;

namespace App.Agent
{
    using Common;
    using Common.Message;

    public interface IDeckAgent
        : IAgent<Model.IDeckModel>
    {
        IFuture<Response> NewGame();
        IFuture<ICardAgent> Draw();
    }
}
