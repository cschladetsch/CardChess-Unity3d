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
            _Coro = New.Coroutine(Next);
            _Node = New.Node(_Coro);
            Kernel.Root.Add(_Node);
            return Construct();
        }

        protected virtual bool Construct()
        {
            return true;
        }

        protected abstract IEnumerator Next(IGenerator self);

        protected IGenerator _Coro;
        protected INode _Node;
    }
}
