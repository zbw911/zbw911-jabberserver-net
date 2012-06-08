using System;
using System.Runtime.Serialization;
using agsXMPP.protocol;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberStreamException : JabberException
    {
        private readonly StreamErrorCondition error;


        public JabberStreamException(StreamErrorCondition error, XmppSession session)
            : base(session)
        {
            this.error = error;
        }


        protected JabberStreamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
            return new Error(error) { Text = Message };
        }
    }
}
