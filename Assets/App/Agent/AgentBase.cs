using System.Collections;
using System.Collections.Generic;
using Flow;
using UnityEngine;
using UnityEngine.Assertions;

namespace  App.Agent
{
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
            if (Completed != null)
                Completed(this);
            Active = false;
        }
    }

    public class AgentBase : AgentLogger
    {
    }

    public interface IAgent<in T0>
    {
        bool Create(T0 t0);
    }

    public abstract class AgentCoroBase<IModel> : Flow.Impl.Coroutine, IAgent<IModel> where IModel : class
    {
        public IModel Model { get { return _model; } }

        public bool Create(IModel model)
        {
            Assert.IsNotNull(model);
            _model = model;
            return Create();
        }

        protected abstract IEnumerator Next();

        protected abstract bool Create();

        protected IModel _model;
    }
}
