using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C.Presences
{
    class PresenceUnsubscribeHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.unsubscribe)]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var ri = context.Storages.Users.GetRosterItem(session.Jid, element.To);
            if (ri == null)
            {
                return Error(session, ErrorCondition.ItemNotFound, element);
            }

            if (ri.Subscription == SubscriptionType.both || ri.Subscription == SubscriptionType.to)
            {
                element.SwitchDirection();
                element.Type = PresenceType.subscribed;
                return Send(session, element);
            }
            else
            {
                ri.Ask = AskType.subscribe;
                context.Storages.Users.SaveRosterItem(session.Jid, ri);

                // TODO: resend if no answer
                var result = Component(Send(context.Sessions.GetSessions(element.To.BareJid), element));
                result.Add(new RosterPush(session.Jid, ri, context));
                return result;
            }
        }
    }
}
