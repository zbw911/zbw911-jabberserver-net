using System;
using System.Diagnostics;

namespace Jabber.Net.Server
{
    static class Log
    {
        public static void Information(string message)
        {
            Trace.TraceInformation(message);
        }

        public static void Information(string format, params object[] args)
        {
            Trace.TraceInformation(format, args);
        }

        public static void Warning(string message)
        {
            Trace.TraceWarning(message);
        }

        public static void Warning(string format, params object[] args)
        {
            Trace.TraceWarning(format, args);
        }

        public static void Error(Exception error)
        {
            Trace.TraceError(error.ToString());
        }

        public static void Error(string format, params object[] args)
        {
            Trace.TraceError(format, args);
        }

    }
}
