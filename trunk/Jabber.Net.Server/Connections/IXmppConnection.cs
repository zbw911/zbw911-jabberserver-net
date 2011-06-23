using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        string Id
        {
            get;
        }

        bool Closed
        {
            get;
        }


        void Recieve(IXmppReciever reciever);

        void Send(byte[] buffer);

        void Close();
    }
}
