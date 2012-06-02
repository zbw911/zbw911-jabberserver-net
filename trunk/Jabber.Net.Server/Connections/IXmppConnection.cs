using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        void BeginRecieve(IXmppReciever reciever);

        void Send(byte[] buffer, Action<byte[]> error);

        void Close();
    }
}
