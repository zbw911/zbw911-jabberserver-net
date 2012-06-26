using agsXMPP;

namespace Jabber.Net.Server.Storages
{
    class XmppUserStorage : IXmppUserStorage
    {
        public XmppUser GetUser(Jid jid)
        {
            return new XmppUser("nikolay.ivanov", "111111");
        }
    }
}
