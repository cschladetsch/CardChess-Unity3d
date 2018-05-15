using UnityEngine;

namespace App.View
{
    public class LoggingBehavior : MonoBehaviour, Flow.ILogger
    {
        public string Prefix { get; set; }
        public int Verbosity { get; set; }
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

        readonly Flow.Impl.Logger _log = new Flow.Impl.Logger("UNITY");
    }
}
