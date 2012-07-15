using agsXMPP.protocol.extensions.bosh;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class BoshHandler : XmppHandler, IXmppHandler<Body>
    {
        public XmppHandlerResult ProcessElement(Body element, XmppSession session, XmppHandlerContext context)
        {
            session.Connection.Reset();

            

            return Void();
        }
    }
}
