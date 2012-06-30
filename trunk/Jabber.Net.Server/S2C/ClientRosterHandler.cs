﻿using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientRosterHandler : XmppHandler, IXmppHandler<RosterIq>
    {
        [IQType(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(RosterIq element, XmppSession session, XmppHandlerContext context)
        {
            var to = element.HasTo ? element.To : session.Jid;
            if (to != session.Jid)
            {
                return Error(session, ErrorCondition.Forbidden, element);
            }

            if (element.Type == IqType.get)
            {
                foreach (var ri in context.Storages.Users.GetRosterItems(to.User))
                {
                    element.Query.AddRosterItem(ri);
                }
                session.RosterRequest();
                return Send(session, element.ToResult());
            }
            else
            {
                if (element.Query.Items.Count() != 1)
                {
                    return Error(session, ErrorCondition.BadRequest, element);
                }

                var ri = element.Query.Items.ElementAt(0);
                var result = Component();

                // roster push
                foreach (var s in context.Sessions.BareSessions(session.Jid))
                {
                    var push = new RosterIq { Type = IqType.set, From = session.Jid.BareJid, To = s.Jid, Query = new Roster() };
                    push.Query.AddRosterItem(ri);
                    result.Add(Send(s, push));
                }

                if (ri.Subscription == SubscriptionType.remove)
                {
                    var item = context.Storages.Users.GetRosterItem(to.User, ri.Jid.BareJid);
                    if (item != null)
                    {
                        context.Storages.Users.RemoveRosterItem(to.User, ri.Jid);

                        if (item.Subscription == SubscriptionType.both || item.Subscription == SubscriptionType.to)
                        {
                            //context.Handlers.ProcessElement(session.EndPoint, Presence.Unsubscribe(session.Jid.BareJid, ri.Jid.BareJid));
                        }
                        if (item.Subscription == SubscriptionType.both || item.Subscription == SubscriptionType.from)
                        {
                            //context.Handlers.ProcessElement(session.EndPoint, Presence.Unsubscribed(session.Jid.BareJid, ri.Jid.BareJid));
                        }
                    }
                    else
                    {
                        return Error(session, ErrorCondition.ItemNotFound, element);
                    }
                }
                else
                {
                    ri.RemoveTag("subscription"); // ignore subscription
                    ri.RemoveTag("ask"); // ignore ask
                    context.Storages.Users.SaveRosterItem(to.User, ri);
                }

                element.Query.Remove();
                result.Add(Send(session, element.ToResult()));
                return result;
            }
        }
    }
}