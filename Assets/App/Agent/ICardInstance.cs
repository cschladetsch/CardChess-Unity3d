using App.Model;

namespace App.Agent
{
    public interface ICardInstance : IAgent<Model.ICardInstance>, IHasId, IHasName
    {
        int Health { get; }
    }
}
