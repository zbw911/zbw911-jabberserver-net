using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppResultContext
    {
        private readonly XmppHandlerContext context;


        public XmppSession Session
        {
            get;
            private set;
        }

        public XmppSessionManager SessionManager
        {
            get { return context.SessionManager; }
        }

        public XmppHandlerManager HandlerManager
        {
            get { return context.HandlerManager; }
        }

        public XmppResultContext(XmppSession session, XmppHandlerContext context)
        {
            Args.NotNull(session, "session");
            Args.NotNull(context, "context");

            this.Session = session;
            this.context = context;
        }
    }
}
