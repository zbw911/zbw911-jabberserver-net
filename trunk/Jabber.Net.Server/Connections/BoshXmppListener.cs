using System;
using System.Net;
using System.Net.Sockets;

namespace Jabber.Net.Server.Connections
{
    class BoshXmppListener : IXmppListener
    {
        private readonly string listenUri;
        private HttpListener listener;
        private Action<IXmppConnection> newConnection;


        public BoshXmppListener(string listenUri)
        {
            Args.NotNull(listenUri, "listenUri");

            this.listenUri = listenUri;
        }


        public void StartListen(Action<IXmppConnection> newConnection)
        {
            Args.Requires<NotSupportedException>(HttpListener.IsSupported, "HttpListener not supported.");
            Args.NotNull(newConnection, "newConnection");

            this.newConnection = newConnection;
            listener = new HttpListener
            {
                IgnoreWriteExceptions = true,
            };
            listener.Prefixes.Add(listenUri);
            listener.Start();
            BeginGetContext(true);
        }

        public void StopListen()
        {
            if (listener != null)
            {
                if (listener.IsListening)
                {
                    listener.Stop();
                }
                listener = null;
            }
        }


        private void OnAccept(IAsyncResult ar)
        {
            var continueAccept = true;
            try
            {
                var listener = (HttpListener)ar.AsyncState;
                var context = listener.EndGetContext(ar);
                newConnection(new BoshXmppConnection(context));
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
                    BeginGetContext(false);
                }
            }
        }

        private void BeginGetContext(bool throwerror)
        {
            try
            {
                listener.BeginGetContext(OnAccept, listener);
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
