using System;
using System.Runtime.Serialization;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberStanzaException : JabberException
    {
        private readonly ErrorCode error;

        
        public JabberStanzaException (ErrorCode error, XmppSession session)
            : base(session)
        {
            this.error = error;
        }


        protected JabberStanzaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
            return new Error(error) { Message = base.Message };
		}
    }
}
