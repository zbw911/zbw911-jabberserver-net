using agsXMPP.protocol.iq.last;
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

        Last GetLast(string username);

        void SetLast(string username, Last vcard);
    }
}
