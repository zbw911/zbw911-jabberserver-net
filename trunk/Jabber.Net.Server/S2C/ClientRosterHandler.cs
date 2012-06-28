using System.Linq;
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
            //NOTE: 'ver' attribute

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

                //TODO: ignore ask, subscription
                var ri = element.Query.Items.ElementAt(0);
                var result = Component();

                // roster push
                foreach (var s in context.Sessions.BareSessions(session.Jid))
                {
                    var push = new RosterIq { Type = IqType.set, To = s.Jid, Query = new Roster() };
                    push.Query.AddRosterItem(ri);
                    result.Add(Send(s, push));
                }

                if (ri.Subscription != SubscriptionType.remove)
                {
                    context.Storages.Users.SaveRosterItem(to.User, ri);
                }
                else
                {
                    var item = context.Storages.Users.GetRosterItems(to.User).FirstOrDefault(r => r.Jid.BareJid == ri.Jid.BareJid);
                    if (item != null)
                    {
                        var unsub = Presence.Unsubscribe(session.Jid.BareJid, ri.Jid.BareJid);
                        var unsubed = Presence.Unsubscribed(session.Jid.BareJid, ri.Jid.BareJid);
                        var sessions = context.Sessions.BareSessions(item.Jid);
                        var rostered = sessions.Where(s => s.Rostered);
                        if (rostered.Any())
                        {
                            if (item.Subscription == SubscriptionType.to || item.Subscription == SubscriptionType.both)
                            {
                                result.Add(Send(rostered, unsub));
                            }
                            if (item.Subscription == SubscriptionType.from || item.Subscription == SubscriptionType.both)
                            {
                                result.Add(Send(rostered, unsubed));
                            }
                        }
                        else if (sessions.Any())
                        {
                            if (item.Subscription == SubscriptionType.to || item.Subscription == SubscriptionType.both)
                            {
                                context.Storages.Elements.SaveElements(ri.Jid, "offline", unsub);
                            }
                            if (item.Subscription == SubscriptionType.from || item.Subscription == SubscriptionType.both)
                            {
                                context.Storages.Elements.SaveElements(ri.Jid, "offline", unsubed);
                            }
                        }
                        result.Add(Send(sessions, Presence.Unavailable(session.Jid.BareJid, ri.Jid.BareJid)));
                    }
                    else
                    {
                        return Error(session, ErrorCondition.ItemNotFound, element);
                    }
                }

                element.Query.Remove();
                result.Add(Send(session, element.ToResult()));
                return result;
            }
        }
    }
}
