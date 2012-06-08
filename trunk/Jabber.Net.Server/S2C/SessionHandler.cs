using System;
using agsXMPP.protocol.iq.session;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class SessionHandler : XmppHandlerBase,
        IXmppHandler<SessionIq>
    {
        public XmppHandlerResult ProcessElement(SessionIq element, XmppSession session, XmppHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
