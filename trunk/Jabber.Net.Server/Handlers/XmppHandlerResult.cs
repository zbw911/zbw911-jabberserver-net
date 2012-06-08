using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public abstract class XmppHandlerResult
    {
        public XmppSession Session
        {
            get;
            private set;
        }


        public XmppHandlerResult()
            : this(XmppSession.Current)
        {

        }

        public XmppHandlerResult(XmppSession session)
        {
            Session = session;
        }


        public abstract void Execute(XmppResultContext context);
    }
}
