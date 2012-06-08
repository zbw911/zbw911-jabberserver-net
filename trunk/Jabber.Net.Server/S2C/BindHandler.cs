using System;
using agsXMPP.protocol.iq.bind;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class BindHandler : XmppHandlerBase,
        IXmppHandler<BindIq>
    {
        public XmppHandlerResult ProcessElement(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
