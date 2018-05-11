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
        bool Create(IFactory factory, IModel model);
    }

	public abstract class AgentCoroBase<IModel> : IAgent<IModel> where IModel : class
    {
        public IModel Model => _model;

        public bool Create(IFactory factory, IModel model)
        {
            Assert.IsNotNull(model);
			New = factory;
			_model = model;
			_coro = factory.Coroutine(Next);

            return Create();
        }

		protected abstract IEnumerator Next(IGenerator self);
        protected abstract bool Create();

		protected IFactory New;
        protected IModel _model;
		protected IGenerator _coro;
    }
}
