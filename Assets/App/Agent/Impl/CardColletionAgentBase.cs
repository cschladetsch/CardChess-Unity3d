namespace App.Agent
{
    using Model;
    using Dekuple.Common;

    /// <summary>
    /// Common to all card collection agents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CardColletionAgentBase<T>
        : AgentBase<T, IModel>
        where T : class, IModel, IHasDestroyHandler<T>
    {
        public CardColletionAgentBase(T model)
            : base(model)
        {
        }
    }
}
