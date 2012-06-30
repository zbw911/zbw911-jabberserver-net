using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    public class PresenceFilterAttribute : XmppValidationAttribute
    {
        private readonly PresenceType[] allowed;


        public PresenceFilterAttribute(params PresenceType[] allowed)
        {
            this.allowed = allowed;
        }

        public override XmppHandlerResult ValidateElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            var presence = element as Presence;
            if (presence.HasTo && presence.To.IsFull)
            {
                presence.To = presence.To.BareJid;
            }
            presence.From = session.Jid.BareJid;

            if (presence.HasTo && !allowed.Contains(presence.Type))
            {
                return Fail();
            }

            if (presence.To.Server != session.Jid.Server)
            {
                return Error(session, ErrorCondition.RemoteServerNotFound, presence);
            }

            return Success();
        }
    }
}
