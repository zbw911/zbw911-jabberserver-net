using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C.Presences
{
    class PresenceUnsubscribedHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.unsubscribed)]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();

            // contact server
            var ri = context.Storages.Users.GetRosterItem(session.Jid, element.To);
            if (ri != null && (ri.Subscription == SubscriptionType.from || ri.Subscription == SubscriptionType.both))
            {
                if (ri.Subscription == SubscriptionType.from)
                {
                    ri.Subscription = SubscriptionType.none;
                }
                if (ri.Subscription == SubscriptionType.both)
                {
                    ri.Subscription = SubscriptionType.to;
                }
                context.Storages.Users.SaveRosterItem(session.Jid, ri);
                result.Add(new RosterPush(session.Jid, ri, context));
            }

            // user server
            ri = context.Storages.Users.GetRosterItem(element.To, session.Jid);
            if (ri != null &&
                ((ri.Subscription == SubscriptionType.to || ri.Subscription == SubscriptionType.both) ||
                (ri.Subscription == SubscriptionType.none && ri.Ask == AskType.subscribe)))
            {
                if (ri.Subscription == SubscriptionType.to)
                {
                    ri.Subscription = SubscriptionType.none;
                }
                if (ri.Subscription == SubscriptionType.both)
                {
                    ri.Subscription = SubscriptionType.from;
                }
                if (ri.Subscription == SubscriptionType.none && ri.Ask == AskType.subscribe)
                {
                    ri.Ask = AskType.NONE;
                }
                context.Storages.Users.SaveRosterItem(session.Jid, ri);

                foreach (var s in context.Sessions.GetSessions(session.Jid.BareJid))
                {
                    // unavailable
                    result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), Presence.Unsubscribe(s.Jid, element.To)));
                }
                result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), element));
                result.Add(new RosterPush(element.To, ri, context));
            }

            return result;
        }
    }
}
