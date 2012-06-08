using System;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppErrorHandler : IXmppErrorHandler
    {
        public XmppHandlerResult OnError(Exception error, XmppSession session, XmppHandlerContext context)
        {
            return new XmppErrorResult(error, session);
        }
    }
}
