using System;
using System.Runtime.Serialization;

namespace System.Diagnostics.Contracts
{
    [Serializable]
    public class ContractException : Exception
    {
        public ContractException()
            : base()
        {
        }

        public ContractException(string message)
            : base(message)
        {
        }

        public ContractException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ContractException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
