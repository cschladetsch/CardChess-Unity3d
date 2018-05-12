using System;
using System.Collections;
using App.Model;
using UnityEngine.Assertions;
using Flow;

namespace App.Agent
{
    /// <inheritdoc />
    /// <summary>
    /// Base for all agents. Provides a custom logger and an ITransient implementation
    /// to be used with Flow library.
    /// </summary>
    public class AgentLogger : Flow.ITransient
    {
        public event TransientHandler Completed;
        public bool Active { get; private set; }
        public IKernel Kernel { get; set; }

        public AgentLogger()
        {
            Active = true;
        }

        public void Complete()
        {
            if (!Active)
                return;
            Completed?.Invoke(this);
            Active = false;
        }
    }

    public abstract class AgentBase<TModel> :
        AgentLogger, IAgent<TModel>,
        ICreated<TModel>
        where TModel : class, App.Model.IModel
    {
        public Guid Id { get; private set;}
        public IModel BaseModel { get; }
        public TModel Model { get; private set; }

        public virtual bool Create(TModel a0)
        {
            Assert.IsNotNull(a0);
            Id = Guid.NewGuid();
            Model = a0;
            return true;
        }
    }

    public abstract class AgentBaseCoro<TModel> : AgentBase<TModel> where TModel : class, App.Model.IModel
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
