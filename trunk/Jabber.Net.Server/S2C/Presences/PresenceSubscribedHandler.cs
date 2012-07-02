using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C.Presences
{
    class PresenceSubscribedHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.subscribed)]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();

            // contact server
            var ri = context.Storages.Users.GetRosterItem(session.Jid, element.To);
            if (ri == null)
            {
                ri = new RosterItem(element.To);
                if (element.Nickname != null && !string.IsNullOrEmpty(element.Nickname.Value))
                {
                    ri.Name = element.Nickname.Value;
                }
            }
            if (ri.Subscription == SubscriptionType.none || ri.Subscription == SubscriptionType.to)
            {
                if (ri.Subscription == SubscriptionType.none)
                {
                    ri.Subscription = SubscriptionType.from;
                }
                if (ri.Subscription == SubscriptionType.to)
                {
                    ri.Subscription = SubscriptionType.both;
                }
                context.Storages.Users.SaveRosterItem(session.Jid, ri);
                result.Add(new RosterPush(session.Jid, ri, context));
            }

            // user server
            ri = context.Storages.Users.GetRosterItem(element.To, session.Jid);
            if (ri != null && (ri.Subscription == SubscriptionType.none || ri.Subscription == SubscriptionType.from) && ri.Ask == AskType.subscribe)
            {
                if (ri.Subscription == SubscriptionType.none)
                {
                    ri.Subscription = SubscriptionType.to;
                }
                if (ri.Subscription == SubscriptionType.from)
                {
                    ri.Subscription = SubscriptionType.both;
                }
                ri.Ask = AskType.NONE;
                context.Storages.Users.SaveRosterItem(element.To, ri);

                result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), element));
                result.Add(new RosterPush(element.To, ri, context));
                foreach (var s in context.Sessions.GetSessions(session.Jid.BareJid))
                {
                    // available
                    result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), Presence.Available(s.Jid, element.To)));
                }
            }

            return result;
        }
    }
}
