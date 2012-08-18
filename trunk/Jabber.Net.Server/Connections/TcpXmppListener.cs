using System;
using System.Net;
using System.Net.Sockets;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppListener : IXmppListener
    {
        private readonly IPEndPoint endpoint;
        private TcpListener listener;
        private Action<IXmppConnection> newConnection;


        public TcpXmppListener(Uri listenUri)
        {
            Args.NotNull(listenUri, "listenUri");

            endpoint = new IPEndPoint(IPAddress.Parse(listenUri.Host), listenUri.Port);
        }


        public void StartListen(Action<IXmppConnection> newConnection)
        {
            Args.NotNull(newConnection, "newConnection");

            this.newConnection = newConnection;
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
            }
        }


        private void OnAccept(IAsyncResult ar)
        {
            var continueAccept = true;
            try
            {
                var listener = (TcpListener)ar.AsyncState;
                var tcpClient = listener.EndAcceptTcpClient(ar);
                newConnection(new TcpXmppConnection(tcpClient));
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
