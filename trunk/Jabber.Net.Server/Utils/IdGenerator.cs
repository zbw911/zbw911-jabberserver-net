﻿using System.Threading;

namespace Jabber.Net.Server.Utils
{
    static class IdGenerator
    {
        private static int counter = 0;


        public static string NewId()
        {
            return Interlocked.Increment(ref counter).ToString();
        }
    }
}
