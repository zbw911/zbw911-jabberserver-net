using agsXMPP.protocol.tls;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class TlsHandler : XmppHandler,
        IXmppHandler<StartTls>,
        IXmppHandler<Proceed>,
        IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Handlers.SupportTls = true;
        }


        public XmppHandlerResult ProcessElement(StartTls element, XmppSession session, XmppHandlerContext context)
        {
            return Void();
        }

        public XmppHandlerResult ProcessElement(Proceed element, XmppSession session, XmppHandlerContext context)
        {
            return Void();
        }
    }
}
