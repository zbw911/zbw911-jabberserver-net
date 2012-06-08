using System;
using System.Runtime.Serialization;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberStanzaException : JabberException
    {
        private readonly ErrorCode error;

        
        public JabberStanzaException (ErrorCode error)
            : base()
        {
            this.error = error;
        }


        protected JabberStanzaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
			return new Error(error);
		}
    }
}
