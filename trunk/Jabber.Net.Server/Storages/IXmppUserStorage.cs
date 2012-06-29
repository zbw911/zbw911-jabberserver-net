using System.Collections.Generic;
using agsXMPP;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppUserStorage
    {
        XmppUser GetUser(string username);

        void SaveUser(XmppUser user);

        bool RemoveUser(string username);


        Vcard GetVCard(string username);

        void SetVCard(string username, Vcard vcard);


        IEnumerable<RosterItem> GetRosterItems(string username);

        RosterItem GetRosterItem(string username, Jid jid);

        void SaveRosterItem(string username, RosterItem ri);

        bool RemoveRosterItem(string username, Jid jid);
    }
}
