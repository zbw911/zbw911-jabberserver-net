using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using agsXMPP;
using System;

namespace Jabber.Net.Server.S2C
{
    class ClientPresenceHandler : XmppHandler, IXmppHandler<Presence>
    {
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            if (element.HasTo && element.To.IsFull)
            {
                element.To = element.To.BareJid;
            }
            if (element.HasTo && element.Type == PresenceType.subscribe)
            {
                if (element.To.Server != session.Jid.Server)
                {
                    //if (true)
                    //{
                    // route to another server 
                    //}
                    //else
                    //{
                    return Error(session, ErrorCondition.RemoteServerNotFound, element);
                    //}
                }
                else
                {
                    if (context.Storages.Users.GetUser(element.To.User) != null)
                    {


                    }
                    else
                    {
                        return Error(session, ErrorCondition.ItemNotFound, element);
                    }
                }
                var ri = new RosterItem(session.Jid.BareJid)
                {
                    Ask = AskType.subscribe,
                    Subscription = SubscriptionType.none,
                };
                var push = RosterPush(element.To, ri, context);
            }

            return Void();
        }

        private XmppHandlerResult RosterPush(Jid to, RosterItem ri, XmppHandlerContext context)
        {
            var result = Component();
            // roster push
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
