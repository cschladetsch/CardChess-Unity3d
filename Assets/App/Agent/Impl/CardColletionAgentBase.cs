using App.Model;

namespace App.Agent
{
    class CardColletionAgentBase<T>
        : AgentBase<T>
        where T : class, IModel
    {
        public CardColletionAgentBase(T model) : base(model)
        {
        }
    }
}
