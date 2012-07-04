﻿using System.Linq;
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
                    session.Presence = element;

                    // send offline stanzas
                    foreach (var jid in context.Storages.Users.GetAskers(session.Jid))
                    {
                        result.Add(Send(session, Presence.Subscribe(jid, session.Jid)));
                    }
                    foreach (var e in context.Storages.Elements.GetElements(session.Jid, "offline%"))
                    {
                        result.Add(Send(session, e, true));
                    }
                    context.Storages.Elements.RemoveElements(session.Jid, "offline%");
                }

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
            }
            return result;
        }
    }
}
