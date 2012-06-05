using System;
using agsXMPP.protocol;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientStreamHandler : IXmppHandler
    {
        public void Register(XmppHandlerManager handlerManager)
        {
            
        }

        
        public XmppHandlerResult ProcessElement(Stream element, XmppSession session, XmppHandlerContext context)
        {
            throw new NotImplementedException();
        }

    }
}
