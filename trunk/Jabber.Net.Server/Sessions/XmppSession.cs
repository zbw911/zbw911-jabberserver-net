using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSession
    {
        public string Id
        {
            get;
            set;
        }

        public IXmppEndPoint EndPoint
        {
            get;
            set;
        }

        public bool Authenticated
        {
            get;
            private set;
        }


        public XmppSession(IXmppEndPoint endpoint)
        {
            EndPoint = endpoint;
        }
    }
}
