// define this for some diagnostics
// #define LOG_TRACE_VERBOSE

// ReSharper disable InconsistentNaming

namespace Dekuple
{
    /// <summary>
    /// Global game parameters.
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// Default logging values.
        /// </summary>
#if LOG_TRACE_VERBOSE
        //public static bool DefaultShowTraceStack = true;
        public static bool DefaultShowTraceStack = false;
        public static bool DefaultShowTraceSource = true;
        public static int DefaultLogVerbosity = 100;
#else
        public static bool DefaultShowTraceStack = false;
        public static bool DefaultShowTraceSource = true;
        public static int DefaultLogVerbosity = 4;
#endif

    }
}
