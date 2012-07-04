using System.Linq;
using agsXMPP.protocol.client;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C.Presences
{
    class PresenceUnavailableHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.unavailable)]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();
            if (!element.HasTo)
            {
                // send to itself available resource
                foreach (var s in context.Sessions.GetSessions(element.From.BareJid).Where(s => s.Available))
                {
                    var p = (Presence)element.Clone();
                    p.To = s.Jid;
                    result.Add(Send(s, p));
                }

                // broadcast to subscribers
                foreach (var to in context.Storages.Users.GetSubscribers(session.Jid))
                {
                    foreach (var s in context.Sessions.GetSessions(to.BareJid).Where(s => s.Available))
                    {
                        var p = (Presence)element.Clone();
                        p.To = s.Jid;
                        result.Add(Send(s, p));
                    }
                }

                session.Presence = element;
            }
            return result;
        }
    }
}
