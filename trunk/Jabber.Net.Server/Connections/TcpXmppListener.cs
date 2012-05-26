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
            Args.NotNull(connectionManager, "connectionManager");

            this.connectionManager = connectionManager;

            var endpoint = new IPEndPoint(IPAddress.Parse(ListenUri.Host), ListenUri.Port);
            listener = new TcpListener(endpoint)
            {
                ExclusiveAddressUse = true,
            };

            listener.Start();
            BeginAcceptTcpClient(true);
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
            var continueAccept = true;
            try
            {
                var listener = (TcpListener)ar.AsyncState;
                var tcpClient = listener.EndAcceptTcpClient(ar);
                connectionManager.AddConnection(new TcpXmppConnection(tcpClient));
            }
            catch (ObjectDisposedException)
            {
                //ignore
                continueAccept = false;
            }
            catch (Exception error)
            {
                Log.Error(error);
            }
            finally
            {
                if (continueAccept)
                {
                    BeginAcceptTcpClient(false);
                }
            }
        }

        private void BeginAcceptTcpClient(bool throwerror)
        {
            try
            {
                listener.BeginAcceptTcpClient(OnAccept, listener);
            }
            catch (ObjectDisposedException)
            {
                //ignore
            }
            catch (Exception error)
            {
                if (throwerror) throw;
                else Log.Error(error);
            }
        }
    }
}
