using System;
using System.Net;
using System.Net.Sockets;
using Jabber.Net.Server.Streams;
using System.Collections.Generic;

namespace Jabber.Net.Server.Listeners
{
    class TcpXmppListener : IXmppListener
    {
        private TcpListener listener;
        private Action<XmppStream> accept;


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


        public void StartListen(Action<XmppStream> accept)
        {
            if (listener == null)
            {
                this.accept = accept;
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
                accept = null;
            }
        }


        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                var listener = (TcpListener)ar.AsyncState;
                listener.BeginAcceptTcpClient(OnAccept, listener);

                var client = listener.EndAcceptTcpClient(ar);
                accept(null);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
    }
}
