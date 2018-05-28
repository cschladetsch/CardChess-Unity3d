using App.Common;
using App.Model;

namespace App.Agent
{
    public interface IArbiterAgent
        : IAgent<IArbiterModel>
    {
        ICardAgent NewCardAgent(ICardModel cardModel, IOwner owner);
    }
}
