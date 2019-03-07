namespace App.Agent
{
    using Model;
    using Dekuple.Common;
    using Dekuple.Registry;

    /// <summary>
    /// Each agent represents a model, has a Flow process, and its own log.
    /// </summary>
    public interface IAgent
        : Dekuple.Agent.IAgent
        , IHasRegistry<IAgent>
        , IHasDestroyHandler<IAgent>
    {
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
        //TModel Model { get; }
    }
}
