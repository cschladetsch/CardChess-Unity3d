using System.Collections;
using App.Model;
using Flow;

namespace App.Agent
{
    using Registry;

    /// <summary>
    /// Base for agents that perform actions over time.
    /// </summary>
    /// <typeparam name="TModel">The model that this Agent represents</typeparam>
    public abstract class AgentBaseCoro<TModel>
        : AgentBase<TModel>
        where TModel : class, IModel
    {
        public override bool Construct(TModel a0)
        {
            base.Construct(a0);
            Coro = New.Coroutine(Next);
            Node = New.Node(Coro);
            Kernel.Root.Add(Node);
            return Construct();
        }

        protected virtual bool Construct()
        {
            return true;
        }

        protected abstract IEnumerator Next(IGenerator self);

        protected IGenerator Coro;
        protected INode Node;
    }
}
