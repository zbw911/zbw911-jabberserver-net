using System;

namespace System.Diagnostics.Contracts
{
    static class Contract
    {
        public static void Requires<T>(bool condition) where T : Exception
        {
            Requires<T>(condition, "Contract failed.");
        }

        public static void Requires<T>(bool condition, string message) where T : Exception
        {
            if (!condition)
            {
                Exception exception = null;
                try
                {
                    exception = (T)Activator.CreateInstance(typeof(T), message);
                }
                catch (MissingMethodException)
                {
                    exception = new ContractException(message);
                }
                throw exception;
            }
        }
    }
}
