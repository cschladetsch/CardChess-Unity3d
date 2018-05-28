using System;
using Flow;

namespace App.Agent
{
    using Common;
    using Registry;

    /// <summary>
    /// AgentBase for all agents. Provides a custom logger and an ITransient implementation
    /// to be used with Flow library.
    /// </summary>
    public class AgentLogger
        : ITransient
        , ILogger
    {
        public event TransientHandler Completed;

        #region Public Properties
        public bool Active { get; private set; }
        public IKernel Kernel { get; set; }
        public IFactory Factory => Kernel.Factory;
        public Flow.IFactory New => Kernel.Factory;
        public Flow.INode Root => Kernel.Root;
        public string Name { get; set; }
        [Inject] public IBoardAgent Board { get; set; }
        [Inject] public IArbiterAgent Arbiter { get; set; }
        public string LogPrefix { get { return _log.LogPrefix; } set { _log.LogPrefix = value; }}
        public object LogSubject { get { return _log.LogSubject; } set { _log.LogSubject = value; } }
        public int Verbosity { get { return _log.Verbosity; } set { _log.Verbosity = value; }}
        #endregion

        #region Public Methods
        public ITransient Named(string name)
        {
            Name = name;
            return this;
        }

        public void Complete()
        {
            if (!Active)
                return;
            Completed?.Invoke(this);
            Active = false;
        }

        public void Info(string fmt, params object[] args)
        {
            _log.Info(fmt, args);
        }

        public void Warn(string fmt, params object[] args)
        {
            _log.Warn(fmt, args);
        }

        public void Error(string fmt, params object[] args)
        {
            _log.Error(fmt, args);
        }

        public void Verbose(int level, string fmt, params object[] args)
        {
            _log.Verbose(level, fmt, args);
        }
        #endregion

        #region Protected
        protected AgentLogger()
        {
            _log.LogSubject = this;
            _log.LogPrefix = "Agent";

        }

        protected readonly LoggerFacade<Flow.Impl.Logger> _log = new LoggerFacade<Flow.Impl.Logger>("Agent");
        #endregion
    }

    public abstract class AgentLogger<TModel>
        : AgentLogger
        where TModel : Model.IModel
    {
        public event DestroyedHandler<IAgent> OnDestroy;

        public Model.IModel BaseModel { get; private set; }
        public TModel Model { get; private set; }
        public IOwner Owner { get; private set; }

        public bool Construct(TModel a0, IOwner owner)
        {
            Model = a0;
            BaseModel = a0;
            Owner = owner;
            return true;
        }
    }

}
