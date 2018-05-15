using Flow;

namespace App.Agent
{
    /// <summary>
    /// AgentBase for all agents. Provides a custom logger and an ITransient implementation
    /// to be used with Flow library.
    /// </summary>
    public class AgentLogger : ITransient, ILogger
    {
        public event TransientHandler Completed;
        public bool Active { get; private set; }
        public IKernel Kernel { get; set; }
        public string Name { get; set; }
        public string Prefix { get { return _log.Prefix; } set { _log.Prefix = value; }}

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

        private LoggerFacade<App.Model.Logger> _log = new LoggerFacade<Model.Logger>("Agent");
    }
}
