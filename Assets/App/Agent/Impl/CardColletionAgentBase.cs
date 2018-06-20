namespace App.Agent
{
    using Model;

    /// <summary>
    /// Common to all card collection agents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CardColletionAgentBase<T>
        : AgentBase<T>
        where T : class, IModel
    {
        public CardColletionAgentBase(T model)
            : base(model)
        {
        }
    }
}
