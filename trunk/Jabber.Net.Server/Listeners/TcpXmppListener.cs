using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Listeners
{
    class TcpXmppListener : IXmppListener
    {
        private TcpListener listener;
        private XmppConnectionManager connectionManager;


        public Uri ListenUri
        {
            get;
            set;
        }

        public int MaxReceivedMessageSize
        {
            get;
            set;
        }


        public void Configure(IDictionary<string, string> properties)
        {

        }


        public void StartListen(XmppConnectionManager connectionManager)
        {
            if (listener == null)
            {
                this.connectionManager = connectionManager;
                listener = new TcpListener(new IPEndPoint(IPAddress.Parse(ListenUri.Host), ListenUri.Port))
                {
                    ExclusiveAddressUse = true,
                };
                listener.Start();
                listener.BeginAcceptTcpClient(OnAccept, listener);
            }
        }

        public void StopListen()
        {
            if (listener != null)
            {
                listener.Stop();
                listener = null;
                connectionManager = null;
            }
        }


        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                var listener = (TcpListener)ar.AsyncState;
                listener.BeginAcceptTcpClient(OnAccept, listener);

                connectionManager.AddConnection(new TcpXmppConnection(listener.EndAcceptTcpClient(ar)));
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
    }
}
