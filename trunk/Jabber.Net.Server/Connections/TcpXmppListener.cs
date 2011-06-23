using System;
using System.Net;
using System.Net.Sockets;

namespace Jabber.Net.Server.Connections
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


        public void StartListen(XmppConnectionManager connectionManager)
        {
            if (connectionManager == null) throw new ArgumentNullException("connectionManager");

            if (listener == null)
            {
                this.connectionManager = connectionManager;
                
                var p = new IPEndPoint(IPAddress.Parse(ListenUri.Host), ListenUri.Port);
                listener = new TcpListener(p) { ExclusiveAddressUse = true, };                
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

                var tcpClient = listener.EndAcceptTcpClient(ar);
                connectionManager.AddConnection(new TcpXmppConnection(tcpClient));
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
    }
}
