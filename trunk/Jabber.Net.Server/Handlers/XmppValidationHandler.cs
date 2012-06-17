using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.register;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    class XmppValidationHandler : XmppHandlerBase, IXmppHandler<Stanza>
    {
        public XmppHandlerResult ProcessElement(Stanza element, XmppSession session, XmppHandlerContext context)
        {
            if (!(element is Stream) && !(element is RegisterIq))
            {
                if (!session.Authenticated)
                {
                    return Error(ErrorCode.Unauthorized);
                }
            }

            return Void();
        }
    }
}
