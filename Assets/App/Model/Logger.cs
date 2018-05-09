using System;
using UnityEngine;

namespace App.Model
{
    /// <summary>
    /// Log system used by Models.
    /// </summary>
    public class Logger
    {
        public string Name { get; set; }

        public enum ELevel
        {
            None = 0, Info = 1, Warn = 2, Verbose = 4, Error = 8,
        }

        public static string LogFileName;
        public static ELevel MaxLevel;

        protected ELevel _logLevel;

        public static void Initialise()
        {
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

            log(MakeEntry(level, text));
        }

        private string MakeEntry(ELevel level, string text)
        {
            return string.Format(
                "{0}:type {1}:name {2}:\n\t'{3}'",
                _logPrefix, GetType(), Name, text);
        }

        protected string _logPrefix;
    }
}
