using System;
using agsXMPP.protocol;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientStreamHandler : IXmppHandler<Stream>
    {
        public XmppHandlerResult ProcessElement(Stream element, XmppSession session, XmppHandlerContext context)
        {
            return null;
        }
    }
}
