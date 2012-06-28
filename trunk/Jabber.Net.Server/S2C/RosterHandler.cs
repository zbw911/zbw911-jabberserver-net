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
                if (element.Query.GetRoster().Length != 1)
                {
                    return Error(session, ErrorCondition.BadRequest, element);
                }

                var ri = element.Query.GetRoster()[0];
                var sessions = context.Sessions.FindSessions(session.Jid.BareJid);
                var result = Component(Send(sessions, (Element)element.Clone())); // send all available user's resources

                if (ri.Subscription != SubscriptionType.remove)
                {
                    context.Storages.Users.SaveRosterItem(to.User, ri);
                }
                else
                {
                    context.Storages.Users.RemoveRosterItem(to.User, ri.Jid);

                    var unsubscribe = new Presence(session.Jid, ri.Jid, PresenceType.unsubscribe);
                    var unsubscribed = new Presence(session.Jid, ri.Jid, PresenceType.unsubscribed);
                    var rostered = sessions.Where(s => s.Rostered);
                    if (rostered.Any())
                    {
                        result.AddResult(Send(rostered, unsubscribe, unsubscribed));
                    }
                    else if (sessions.Any())
                    {
                        context.Storages.Elements.SaveElements(ri.Jid, "offline", unsubscribe, unsubscribed);
                    }
                    result.AddResult(Send(sessions, new Presence(session.Jid, ri.Jid, PresenceType.unavailable)));
                }

                element.ToResult();
                result.AddResult(Send(session, element));
                return result;
                //send all available user's resources

                /*catch (Exception)
                {
                    // restore
                    roster.RemoveAllChildNodes();
                    item = context.StorageManager.RosterStorage.GetRosterItem(iq.From, item.Jid);
                    if (item != null)
                    {
                        roster.AddRosterItem(item.ToRosterItem());
                        context.Sender.Broadcast(context.SessionManager.GetBareJidSessions(iq.From), iq);
                    }
                    throw;
                }*/
            }
        }
    }
}
