namespace App.Common
{
    using Agent;
    using Model;

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
