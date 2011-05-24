using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppListener
    {
        Uri ListenUri
        {
            get;
            set;
        }

        int MaxReceivedMessageSize
        {
            get;
            set;
        }


        void StartListen(XmppConnectionManager connectionManager);

        void StopListen();
    }
}
