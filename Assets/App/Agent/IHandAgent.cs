using App.Common.Message;
using Flow;

namespace App.Agent
{
    public interface IHandAgent
        : IAgent<Model.IHandModel>
    {
        IFuture<Response> NewGame();
        IFuture<Response> Add(ICardAgent card);
    }
}
