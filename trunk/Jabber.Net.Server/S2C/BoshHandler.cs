using agsXMPP.protocol.extensions.bosh;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using agsXMPP.protocol.client;

namespace Jabber.Net.Server.S2C
{
    class BoshHandler : XmppHandler, IXmppHandler<Body>
    {
        public XmppHandlerResult ProcessElement(Body element, XmppSession session, XmppHandlerContext context)
        {
            var sid = element.Sid;
            if (string.IsNullOrEmpty(sid))
            {
                return StartBOSHSession(element, session, context);
            }
            else
            {
                var connection = session.Connection;
                session = context.Sessions.GetSession(sid);
            }

            return Void();
        }


        private XmppHandlerResult StartBOSHSession(Body element, XmppSession session, XmppHandlerContext context)
        {
            var stream = new Stream
            {
                Prefix = agsXMPP.Uri.PREFIX,
                DefaultNamespace = agsXMPP.Uri.CLIENT,
                Version = element.XmppVersion,
                To = element.To,
                From = element.From,
                Language = element.GetAttribute("xml:lang"),
            };

            element.Sid = session.Id;
            element.From = element.To;
            element.To = null;

            return Component(Send(session, element), Process(session, stream), Close(session));
        }
    }
}
