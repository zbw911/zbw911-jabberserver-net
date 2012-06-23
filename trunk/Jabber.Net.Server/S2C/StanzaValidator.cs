using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.iq.auth;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.register;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class StanzaValidator : XmppHandler, IXmppValidator<Stanza>
    {
        public XmppHandlerResult ValidateElement(Stanza element, XmppSession session, XmppHandlerContext context)
        {
            // auhtentication
            if (!session.Authenticated && !(element is AuthIq) && !(element is RegisterIq))
            {
                return Error(session, StreamErrorCondition.NotAuthorized);
            }

            // resource binding
            if (!session.Binded && !(element is AuthIq) && !(element is RegisterIq) && !(element is BindIq))
            {
                return Error(session, StreamErrorCondition.NotAuthorized);
            }

            // correct from
            if (element.HasFrom && session.Jid != element.From)
            {
                return Error(session, StreamErrorCondition.InvalidFrom);
            }

            return Void();
        }
    }
}
