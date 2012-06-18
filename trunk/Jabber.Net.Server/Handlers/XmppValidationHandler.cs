using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.iq.register;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    class XmppValidationHandler : XmppHandlerBase, IXmppHandler<Stanza>
    {
        public XmppHandlerResult ProcessElement(Stanza element, XmppSession session, XmppHandlerContext context)
        {
            if (!(element is RegisterIq))
            {
                if (!session.Authenticated)
                {
                    return Error(session, StreamErrorCondition.NotAuthorized);
                }
            }

            return Void();
        }
    }
}
