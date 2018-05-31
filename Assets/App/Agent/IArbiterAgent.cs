using App.Model;
using Flow;

namespace App.Agent
{
    public interface IArbiterAgent
        : IAgent<IArbiterModel>
    {
        void StartGame(IPlayerAgent p0, IPlayerAgent p1);
        ITransient NewGame();
    }
}
