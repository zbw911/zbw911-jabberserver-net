using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppResultContext : XmppHandlerContext
    {
        public XmppSession Session
        {
            get;
            private set;
        }

        public XmppResultContext(XmppSession session, XmppHandlerManager handlers, IXmppResolver resolver)
            : base(handlers, resolver)
        {
            Args.NotNull(session, "session");

            Session = session;
        }
    }
}
