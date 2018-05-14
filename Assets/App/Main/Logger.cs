using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace App
{
    /// <summary>
    /// Log system used by Models.
    /// </summary>
    public class Logger
    {
        #region Public Fields
        public enum ELevel
        {
            None = 0, Info = 1, Warn = 2, Verbose = 4, Error = 8,
        }

        public static string LogFileName;
        public static ELevel MaxLevel;
        public string Name { get; set; }
        #endregion

        #region Protected Methods
        #region Virtuals
        protected virtual string MakeEntry(ELevel level, string text)
        {
            return $"{_logPrefix} type={GetType()}, name={Name}: '{text}'";
        }
        #endregion

        protected void Info(object obj)
        {
            Info($"{obj}");
        }

        protected void Info(string fmt, params object[] args)
        {
            Log(ELevel.Info, string.Format(fmt, args));
        }

        protected void Warn(string fmt, params object[] args)
        {
            Log(ELevel.Warn, string.Format(fmt, args));
        }

        protected void Error(string fmt, params object[] args)
        {
            Log(ELevel.Error, string.Format(fmt, args));
        }
        #endregion

        #region Private Methods
        private void Log(ELevel level, string text)
        {
            Action<string> log = Debug.Log;
            if (level == ELevel.None)
                level = ELevel.Error;
            // TODO: use bitmasks as intended
            switch (level)
            {
                case ELevel.Info:
                    log = Debug.Log;
                    break;
                case ELevel.Warn:
                    log = Debug.LogWarning;
                    break;
                case ELevel.Error:
                    log = Debug.LogError;
                    break;
            }

            #if TRACE
            Trace.WriteLine(MakeEntry(level, text));
            Console.WriteLine(MakeEntry(level, text));
            #else
            log(MakeEntry(level, text));
            #endif
        }
        #endregion

        #region Protected Fields
        protected ELevel _logLevel;
        protected string _logPrefix;
        #endregion
    }
}
