using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.session;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class SessionHandler : XmppHandler, IXmppHandler<SessionIq>, IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Sessions.SupportSession = true;
        }

        [IQType(IqType.set)]
        public XmppHandlerResult ProcessElement(SessionIq element, XmppSession session, XmppHandlerContext context)
        {
            element.ToResult();
            return Send(session, element);
        }
    }
}
