using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSession
    {
        public IXmppEndPoint EndPoint
        {
            get;
            private set;
        }


        public XmppSession(IXmppEndPoint endpoint)
        {
            Args.NotNull(endpoint, "endpoint");

            EndPoint = endpoint;
        }
    }
}
