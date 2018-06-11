namespace App.Agent
{
    using Common;
    using Model;
    using Registry;
    using Flow;

    /// <summary>
    /// AgentBase for all agents. Each agent represents a model and has it's own log.
    /// </summary>
    public interface IAgent
        : ILogger
        , ITransient
        , IEntity
        , IGameActor
        , IHasRegistry<IAgent>
        , IHasDestroyHandler<IAgent>
    {
        IModel BaseModel { get; }
        IArbiterAgent Arbiter { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// A type-specific agent.
    /// </summary>
    /// <typeparam name="TModel">The type of the model this agent represents</typeparam>
    public interface IAgent<out TModel>
        : IAgent
        where TModel : IModel
    {
        TModel Model { get; }
    }
}
