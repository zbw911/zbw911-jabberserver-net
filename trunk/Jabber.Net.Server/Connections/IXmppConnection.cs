using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        string Id
        {
            get;
        }

        void Recieve(IXmppReciever reciever);

        void Send(byte[] buffer);

        void Close();
    }
}
