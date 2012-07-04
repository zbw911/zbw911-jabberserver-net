using System.Linq;
using agsXMPP.protocol.client;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C.Presences
{
    class PresenceAvailableHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.available)]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();
            if (!element.HasTo)
            {
                if (!session.Available)
                {
                    session.Available = true;

                    // send offline stanzas
                    foreach (var p in context.Storages.Users.GetPendingPresences(session.Jid))
                    {
                        result.Add(Send(session, p));
                    }
                    foreach (var e in context.Storages.Elements.GetElements(session.Jid, "offline%"))
                    {
                        result.Add(Send(session, e, true));
                    }
                    context.Storages.Elements.RemoveElements(session.Jid, "offline%");
                }
                session.Priority = element.Priority;

                // broadcast
                foreach (var to in context.Storages.Users.GetToJids(session.Jid))
                {
                    foreach (var s in context.Sessions.GetSessions(to.BareJid).Where(s => s.Available))
                    {
                        var p = (Presence)element.Clone();
                        p.To = s.Jid;
                        result.Add(Send(s, p));
                    }
                }
            }
            return result;
        }
    }
}
