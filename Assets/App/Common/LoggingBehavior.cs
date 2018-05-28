using UnityEngine;

namespace App.Common
{
    public class LoggingBehavior
        : MonoBehaviour
        , Flow.ILogger
    {
        public string LogPrefix { get { return _log.LogPrefix; } set { _log.LogPrefix = value; } }
        public object LogSubject { get { return _log.LogSubject; } set { _log.LogSubject = value; } }
        public int Verbosity { get { return _log.Verbosity; } set { _log.Verbosity = value; } }

        protected LoggingBehavior()
        {
            _log.LogSubject = this;
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

        private readonly Flow.Impl.Logger _log = new Flow.Impl.Logger("UNITY");
    }
}
