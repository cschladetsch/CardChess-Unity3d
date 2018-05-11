using System.Collections;
using UnityEngine.Assertions;
using Flow;

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

    public interface IAgent<IModel> where IModel : class
    {
        IModel Model { get; }
        bool Create(IModel model);
    }

    public abstract class AgentCoroBase<IModel> : Flow.Impl.Coroutine, IAgent<IModel> where IModel : class
    {
        public IModel Model => _model;

        public bool Create(IModel model)
        {
            Start = Next;
            Assert.IsNotNull(model);
            _model = model;
            return Create();
        }

        protected abstract IEnumerator Next();

        protected abstract bool Create();

        protected IModel _model;
    }
}
