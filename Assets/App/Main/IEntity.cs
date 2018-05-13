using App.Agent;
using App.Model;

namespace App
{
    public interface IEntity
    {
        IModel BaseModel { get; }
        IAgent BaseAgent { get; }
    }

    public interface IEntity<TModel, TAgent> : IEntity, ICreateWith<TModel, TAgent>
    {
        TModel Model { get; }
        TAgent Agent { get; }
    }
}
