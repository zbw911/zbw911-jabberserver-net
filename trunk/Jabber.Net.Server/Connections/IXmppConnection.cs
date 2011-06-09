using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        Guid Id
        {
            get;
        }


        void StartRecieve(IXmppReciever reciever);

        void Send(byte[] buffer);

        void Close();
    }
}
