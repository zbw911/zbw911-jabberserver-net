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

        public static void Warning(string message)
        {
            Trace.TraceWarning(message);
        }

        public static void Error(Exception error)
        {
            Trace.TraceError(error.ToString());
        }
    }
}
