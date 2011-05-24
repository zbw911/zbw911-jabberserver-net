using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection
    {
        private TcpClient tcpClient;
        private NetworkStream stream;

        private volatile bool closed;
        private byte[] readBuffer;
        private List<byte[]> notSended;


        public Guid Id
        {
            get;
            private set;
        }


        public event EventHandler<XmppConnectionCloseArgs> Closed;

        public event EventHandler<XmppConnectionRecieveArgs> Recieved;


        public TcpXmppConnection(TcpClient tcpClient)
        {
            Id = Guid.NewGuid();

            this.tcpClient = tcpClient;
            this.stream = tcpClient.GetStream();

            this.readBuffer = new byte[tcpClient.ReceiveBufferSize];
            this.notSended = new List<byte[]>(2);
        }

        public void StartRecieve()
        {
            if (closed) throw new ObjectDisposedException(GetType().FullName);

            stream.BeginRead(readBuffer, 0, readBuffer.Length, ReadCallback, null);
        }

        public void Send(byte[] buffer)
        {
            if (closed) throw new ObjectDisposedException(GetType().FullName);

            stream.BeginWrite(buffer, 0, buffer.Length, SendCallback, buffer);
        }

        public void Close()
        {
            if (closed) return;
            closed = true;

            try
            {
                tcpClient.Close();
            }
            catch { }
            try
            {
                stream.Close();
            }
            catch { }

            var ev = Closed;
            if (ev != null)
            {
                byte[][] buffer = null;
                lock (notSended) buffer = notSended.ToArray();
                ev(this, new XmppConnectionCloseArgs(Id, buffer));
            }
        }


        private void ReadCallback(IAsyncResult ar)
        {
            if (closed) return;
            try
            {
                var readed = stream.EndRead(ar);
                if (0 < readed)
                {
                    var buffer = new byte[readed];
                    Array.Copy(readBuffer, buffer, readed);

                    var ev = Recieved;
                    if (ev != null) ev(this, new XmppConnectionRecieveArgs(Id, buffer));

                    stream.BeginRead(readBuffer, 0, readBuffer.Length, ReadCallback, null);
                }
                else
                {
                    Close();
                }
            }
            catch (Exception)
            {
                Close();
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                stream.EndWrite(ar);
            }
            catch (Exception)
            {
                lock (notSended) notSended.Add((byte[])ar.AsyncState);
                Close();
            }
        }
    }
}
