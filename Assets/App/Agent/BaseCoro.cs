using System.Collections;
using App.Model;
using Flow;

namespace App.Agent
{
    public abstract class BaseCoro<TModel> :
        Base<TModel> where TModel : class, IModel
    {
        public override bool Create(TModel a0)
        {
            base.Create(a0);
            _coro = Arbiter.Kernel.Factory.Coroutine(Next);
            return Construct();
        }

        protected abstract IEnumerator Next(IGenerator self);

        protected virtual bool Construct()
        {
            return true;
        }

        protected IFactory New;
        protected IGenerator _coro;
    }
}
