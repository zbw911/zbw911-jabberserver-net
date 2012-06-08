using Jabber.Net.Server.Connections;
using System;
using agsXMPP;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSession
    {
        private IXmppEndPoint endpoint;


        public string Id
        {
            get;
            private set;
        }

        public Jid Jid
        {
            get;
            set;
        }

        public IXmppEndPoint EndPoint
        {
            get { return endpoint; }
            set
            {
                Args.NotNull(value, "EndPoint");
                endpoint = value;
                endpoint.SessionId = Id;
            }
        }

        public bool Authenticated
        {
            get;
            private set;
        }

        public object AuthData
        {
            get;
            set;
        }


        public XmppSession(IXmppEndPoint endpoint)
        {
            Id = Guid.NewGuid().ToString("N");
            EndPoint = endpoint;
        }

        public void Authenticate(string username)
        {
            Jid.User = username;
            Authenticated = true;
            AuthData = null;
        }
    }
}
