using System;
using Jabber.Net.Server.Configuration;
using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Listeners
{
    public interface IXmppListener : IConfigurable
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
