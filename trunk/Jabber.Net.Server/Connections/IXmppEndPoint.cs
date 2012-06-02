using System;
using Jabber.Net.Xmpp;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppEndPoint
    {
        string SessionId
        {
            get;
            set;
        }


        void Send(XmppElement e, Action<XmppElement> error);

        void Close();
    }
}
