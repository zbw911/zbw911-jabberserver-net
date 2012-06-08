using System;
using System.Runtime.Serialization;
using agsXMPP.protocol;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberSaslException : JabberStreamException
    {
        private readonly FailureCondition error;


        public JabberSaslException(FailureCondition error, XmppSession session)
            : base(StreamErrorCondition.NotAuthorized, session)
        {
            this.error = error;
        }


        protected JabberSaslException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
            return new Failure(error);
        }
    }
}
