using App.Common.Message;

namespace App.Agent
{
    public interface IHandAgent
        : IAgent<Model.IHandModel>
    {
        Response NewGame();
        Response Add(ICardAgent card);
        Response<ICardAgent> DrawCard();
    }
}
