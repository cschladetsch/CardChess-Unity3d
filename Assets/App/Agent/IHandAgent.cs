using Flow;

namespace App.Agent
{
    using Common.Message;

    public interface IHandAgent
        : IAgent<Model.IHandModel>
    {
        IFuture<Response> Add(ICardAgent card);
    }
}
