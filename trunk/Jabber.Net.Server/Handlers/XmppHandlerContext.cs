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

        public XmppHandlerManager HandlerManager
        {
            get;
            private set;
        }

        public XmppHandlerContext(XmppHandlerManager handlerManager, XmppSessionManager sessionManager)
        {
            Args.NotNull(handlerManager, "handlerManager");
            Args.NotNull(sessionManager, "sessionManager");

            HandlerManager = handlerManager;
            SessionManager = sessionManager;
        }
    }
}
