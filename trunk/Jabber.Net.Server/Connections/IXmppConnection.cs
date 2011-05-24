using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        Guid Id
        {
            get;
        }


        event EventHandler<XmppConnectionRecieveArgs> Recieved;

        event EventHandler<XmppConnectionCloseArgs> Closed;


        void StartRecieve();

        void Send(byte[] buffer);

        void Close();
    }
}
