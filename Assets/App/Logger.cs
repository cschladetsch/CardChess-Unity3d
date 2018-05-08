using UnityEngine;

namespace App
{
    /// <summary>
    /// Log system used by agents.
    /// </summary>
    public class Logger : MonoBehaviour
    {
        public enum ELevel
        {
            Info = 1, Warn = 2, Verbose = 4, Error = 8,
        }

        public static string LogFileName;
        public static ELevel MaxLevel;

        protected ELevel _logLevel;

        public static void Initialise()
        {
        }

        protected void Info(string fmt, params object[] args)
        {
        }

        protected void Warn(string fmt, params object[] args)
        {
        }

        protected void Error(string fmt, params object[] args)
        {
        }

        private void Log(ELevel level, string text)
        {
        }

        protected string _logPrefix;
    }
}