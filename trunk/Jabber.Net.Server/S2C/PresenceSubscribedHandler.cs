﻿using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PresenceSubscribedHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.subscribed)]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();

            var ri = context.Storages.Users.GetRosterItem(session.Jid.User, element.To);
            if (ri == null)
            {
                ri = new RosterItem(element.To) { Name = element.GetNickname() };
            }
            if (ri.Subscription == SubscriptionType.none)
            {
                ri.Subscription = SubscriptionType.from;
            }
            if (ri.Subscription == SubscriptionType.to)
            {
                ri.Subscription = SubscriptionType.both;
            }
            context.Storages.Users.SaveRosterItem(session.Jid.User, ri);

            result.Add(RosterPush(session.Jid, ri, context));
            foreach (var s in context.Sessions.BareSessions(session.Jid))
            {
                // available
                result.Add(Send(context.Sessions.BareSessions(element.To), new Presence { To = element.To, From = s.Jid }));
            }

            ri = context.Storages.Users.GetRosterItem(element.To.User, session.Jid);
            if (ri != null)
            {
                if (ri.Subscription == SubscriptionType.none)
                {
                    ri.Subscription = SubscriptionType.to;
                }
                if (ri.Subscription == SubscriptionType.from)
                {
                    ri.Subscription = SubscriptionType.both;
                }
                context.Storages.Users.SaveRosterItem(element.To.User, ri);
            }

            return result;
        }

        private XmppHandlerResult RosterPush(Jid to, RosterItem ri, XmppHandlerContext context)
        {
            var result = Component();
            foreach (var s in context.Sessions.BareSessions(to))
            {
                var push = new RosterIq { Type = IqType.set, To = s.Jid, Query = new Roster() };
                push.Query.AddRosterItem(ri);
                result.Add(Send(s, push));
            }
            return result;
        }
    }
}