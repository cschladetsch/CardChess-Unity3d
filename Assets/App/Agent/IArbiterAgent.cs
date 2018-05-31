using App.Common;
using App.Model;

namespace App.Agent
{
    public interface IArbiterAgent
        : IAgent<IArbiterModel>
    {
        //ICardAgent NewCardAgent(ICardModel cardModel, IOwner owner);
        void StartGame(IPlayerAgent p0, IPlayerAgent p1);
    }
}
