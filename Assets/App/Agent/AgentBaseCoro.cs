using System.Collections;
using App.Model;
using Flow;

namespace App.Agent
{
    /// <summary>
    /// Base for agents that perform actions over time.
    /// </summary>
    /// <typeparam name="TModel">The model that this Agent represents</typeparam>
    public abstract class AgentBaseCoro<TModel> :
        AgentBase<TModel> where TModel : class, IModel
    {
        public override bool Create(TModel a0)
        {
            base.Create(a0);
            _coro = New.Coroutine(Next);
            return Construct();
        }

        protected virtual bool Construct()
        {
            return true;
        }

        protected abstract IEnumerator Next(IGenerator self);

        protected IFactory New => Arbiter.Kernel.Factory;
        protected IGenerator _coro;
    }
}
