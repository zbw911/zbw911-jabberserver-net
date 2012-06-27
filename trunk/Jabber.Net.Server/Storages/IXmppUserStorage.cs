using System.Collections.Generic;
using agsXMPP;
using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppUserStorage
    {
        XmppUser GetUser(string username);

        void SaveUser(XmppUser user);

        void RemoveUser(string username);


        Vcard GetVCard(string username);

        void SetVCard(string username, Vcard vcard);


        IEnumerable<RosterItem> GetRosterItems(string username);

        void SaveRosterItem(string username, RosterItem ri);

        void RemoveRosterItem(string username, Jid jid);
    }
}
