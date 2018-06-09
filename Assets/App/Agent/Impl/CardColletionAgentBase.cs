
namespace App.Agent
{
    using Model;

    class CardColletionAgentBase<T>
        : AgentBase<T>
        where T : class, IModel
    {
        public CardColletionAgentBase(T model) : base(model)
        {
        }
    }
}
