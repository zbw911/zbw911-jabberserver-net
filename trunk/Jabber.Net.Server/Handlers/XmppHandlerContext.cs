using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerContext
    {
        public XmppSessionManager SessionManager
        {
            get;
            private set;
        }

        public XmppHandlerContext(XmppSessionManager sessionManager)
        {
            SessionManager = sessionManager;
        }
    }
}
