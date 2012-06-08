using System;
using System.Runtime.Serialization;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server
{
    [Serializable]
    public abstract class JabberException : Exception
    {
        public XmppSession Session
        {
            get;
            set;
        }

        public abstract bool CloseStream
        {
            get;
        }


        public JabberException()
            : base()
        {
        }

        public JabberException(string message)
            : base(message)
        {
        }

        public JabberException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public JabberException(XmppSession session)
        {
            Session = session;
        }

        protected JabberException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public abstract Element ToElement();
    }
}
