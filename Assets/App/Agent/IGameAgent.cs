using Dekuple;

namespace App.Agent
{
    using Dekuple.Agent;
    using Dekuple.View;

    public interface IGameAgent<out TInterface>
        : IAgent<TInterface>
        where TInterface : Dekuple.Model.IModel
    {
        // void SetAgent(IOwner view, IAgent agent);
        void StartGame();
        void EndGame();
    }
}