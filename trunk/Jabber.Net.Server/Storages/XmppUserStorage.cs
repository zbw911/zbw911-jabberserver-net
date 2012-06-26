using System;
using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.vcard;

namespace Jabber.Net.Server.Storages
{
    class XmppUserStorage : IXmppUserStorage
    {
        public XmppUser GetUser(string username)
        {
            return new XmppUser("nikolay.ivanov", "111111");
        }

        public void SaveUser(XmppUser user)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(string username)
        {
            throw new System.NotImplementedException();
        }

        public Vcard GetVCard(string username)
        {
            throw new System.NotImplementedException();
        }

        public void SetVCard(string username, Vcard vcard)
        {
            throw new NotImplementedException();
        }

        public Last GetLast(string username)
        {
            throw new NotImplementedException();
        }

        public void SetLast(string username, Last vcard)
        {
            throw new NotImplementedException();
        }
    }
}
