namespace App.Agent
{
    /// <summary>
    /// AgentBase for all agents. Each agent represents a model.
    /// </summary>
    public interface IAgent
    {
        Model.IModel BaseModel { get; }
        Arbiter Arbiter { get; set; }
    }

    /// <inheritdoc cref="" />
    /// <summary>
    /// A type-specific agent.
    /// </summary>
    /// <typeparam name="TModel">The type of the model this agent represents</typeparam>
    public interface IAgent<TModel>
        : Common.IConstructWith<TModel>, IAgent
        where TModel : Model.IModel
    {
        TModel Model { get; }
    }
}
