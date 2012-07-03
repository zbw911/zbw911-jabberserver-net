using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C.Presences
{
    class PresenceAttribute : XmppValidationAttribute
    {
        private readonly PresenceType allowed;


        public PresenceAttribute(PresenceType allowed)
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
            if (presence.To == presence.From)
            {
                return Fail();
            }

            if (presence.HasTo && presence.Type == allowed)
            {
                return Success();
            }

            return Fail();
        }
    }
}
