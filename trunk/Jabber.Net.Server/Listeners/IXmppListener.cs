using System;
using Jabber.Net.Server.Streams;
using Jabber.Net.Server.Configuration;

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


        void StartListen(Action<XmppStream> accept);

        void StopListen();
    }
}
