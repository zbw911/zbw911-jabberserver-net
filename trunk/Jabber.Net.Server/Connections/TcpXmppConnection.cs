using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Xmpp;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection, IXmppTlsConnection
    {
        private readonly object locker = new object();
        private volatile bool closed = false;

        private readonly TcpClient client;
        private Stream stream;

        private XmppHandlerManager handlerManager;
        private XmppStreamReader reader;


        public string SessionId
        {
            get;
            set;
        }

        public bool TlsStarted
        {
            get { return !(stream is SslStream); }
        }


        public TcpXmppConnection(TcpClient tcpClient)
        {
            Args.NotNull(tcpClient, "tcpClient");

            client = tcpClient;
            stream = client.GetStream();
        }


        public void BeginReceive(XmppHandlerManager handlerManager)
        {
            RequiresNotClosed();
            Args.NotNull(handlerManager, "handlerManager");

            this.handlerManager = handlerManager;
            Reset();
        }

        public void Reset()
        {
            reader = new XmppStreamReader(stream);
            reader.ReadElementComleted += (s, e) =>
            {
                if (e.State == XmppStreamState.Success)
                {
                    handlerManager.ProcessElement(this, e.Element);
                }
                else if (e.State == XmppStreamState.Error)
                {
                    if (!IgnoreError(e.Error))
                    {
                        Log.Error(e.Error);
                    }
                    Close();
                }
                else if (e.State == XmppStreamState.Closed)
                {
                    Close();
                }
            };
            reader.ReadElementAsync();
        }

        public void Send(Element element, Action<Element> onerror)
        {
            Args.NotNull(element, "element");

            var writer = new XmppStreamWriter(stream);
            writer.WriteElementComleted += (s, e) =>
            {
                if (e.State == XmppStreamState.Error)
                {
                    if (!IgnoreError(e.Error))
                    {
                        Log.Error(e.Error);
                    }
                    try
                    {
                        if (onerror != null)
                        {
                            onerror(element);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                    Close();
                }
            };
            writer.WriteElementAsync(element);
        }

        public void Close()
        {
            lock (locker)
            {
                if (closed) return;
                closed = true;

                try
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                catch (Exception) { }
                try
                {
                    if (client != null)
                    {
                        client.Close();
                    }
                }
                catch (Exception) { }
                try
                {
                    if (handlerManager != null)
                    {
                        handlerManager.ProcessClose(this);
                    }
                }
                catch (Exception) { }
            }
        }

        public void TlsStart(X509Certificate certificate)
        {
            Args.NotNull(certificate, "certificate");

            stream.Flush();
            stream = new SslStream(stream);
            ((SslStream)stream).AuthenticateAsServer(certificate, false, SslProtocols.Ssl3, true);
        }


        private void RequiresNotClosed()
        {
            Args.Requires<ObjectDisposedException>(!closed, GetType().FullName);
        }

        private bool IgnoreError(Exception error)
        {
            return error is ObjectDisposedException || error is IOException;
        }
    }
}
