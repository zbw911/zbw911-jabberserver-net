using System;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    class XmppRequiredHandler : XmppHandlerBase,
        IXmppErrorHandler,
        IXmppCloseHandler
    {
        public XmppHandlerResult OnError(Exception error, XmppSession session, XmppHandlerContext context)
        {
            return Error(error, session);
        }

        public XmppHandlerResult OnClose(XmppSession session, XmppHandlerContext context)
        {
            context.Sessions.CloseSession(session.Id);
            return Void();
        }
    }
}
