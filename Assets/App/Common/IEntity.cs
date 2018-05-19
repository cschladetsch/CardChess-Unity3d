using App.Agent;
using App.Model;

namespace App.Common
{
    public interface IEntity
    {
        IModel BaseModel { get; }
        IAgent BaseAgent { get; }
    }

    public interface IEntity<TModel, TAgent> : IEntity, IConstructWith<TModel, TAgent>
    {
        TModel Model { get; }
        TAgent Agent { get; }
    }
}
