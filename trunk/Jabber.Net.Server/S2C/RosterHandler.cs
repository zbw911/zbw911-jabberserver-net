using System;
using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class RosterHandler : XmppHandler, IXmppHandler<RosterIq>
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
                element.ToResult();
                session.RosterRequest();
                return Send(session, element);
            }
            else
            {
                if (element.Query.Items.Count() != 1)
                {
                    return Error(session, ErrorCondition.BadRequest, element);
                }

                var ri = element.Query.Items.ElementAt(0);
                try
                {
                    var sessions = context.Sessions.FindSessions(session.Jid.BareJid);
                    var result = Component(Send(sessions, (Element)element.Clone())); // send all available user's resources

                    if (ri.Subscription != SubscriptionType.remove)
                    {
                        context.Storages.Users.SaveRosterItem(to.User, ri);
                    }
                    else
                    {
                        context.Storages.Users.RemoveRosterItem(to.User, ri.Jid);

                        var unsub = Presence.Unsubscribe(session.Jid, ri.Jid);
                        var unsubed = Presence.Unsubscribed(session.Jid, ri.Jid);
                        var rostered = sessions.Where(s => s.Rostered);
                        if (rostered.Any())
                        {
                            result.Add(Send(rostered, unsub, unsubed));
                        }
                        else if (sessions.Any())
                        {
                            context.Storages.Elements.SaveElements(ri.Jid, "offline", unsub, unsubed);
                        }
                        result.Add(Send(sessions, Presence.Unavailable(session.Jid, ri.Jid)));
                    }

                    element.ToResult();
                    result.Add(Send(session, element));
                    return result;
                }
                catch (Exception ex)
                {
                    // restore
                    var error = Error(session, ErrorCondition.InternalServerError, element, ex.Message);
                    element.RemoveAllChildNodes();
                    var item = context.Storages.Users.GetRosterItems(session.Jid.User).FirstOrDefault(r => r.Jid == ri.Jid);
                    if (item != null)
                    {
                        element.Query.AddRosterItem(item);
                        return Component(error, Send(context.Sessions.FindSessions(session.Jid.BareJid), element));
                    }
                    return error;
                }
            }
        }
    }
}
