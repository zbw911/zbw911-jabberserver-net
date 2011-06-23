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

        private IXmppReciever reciever;


        public string Id
        {
            get;
            private set;
        }

        public bool Closed
        {
            get { return closed; }
        }


        public TcpXmppConnection(TcpClient tcpClient)
        {
            if (tcpClient == null) throw new ArgumentNullException("tcpClient");

            Id = Guid.NewGuid().ToString("N");

            this.tcpClient = tcpClient;
            this.stream = tcpClient.GetStream();

            this.readBuffer = new byte[tcpClient.ReceiveBufferSize];
            this.notSended = new List<byte[]>(5);
        }


        public void Recieve(IXmppReciever reciever)
        {
            if (closed) throw new ObjectDisposedException(GetType().FullName);
            if (reciever == null) throw new ArgumentNullException("reciever");

            this.reciever = reciever;
            stream.BeginRead(readBuffer, 0, readBuffer.Length, ReadCallback, null);
        }

        public void Send(byte[] buffer)
        {
            if (closed) throw new ObjectDisposedException(GetType().FullName);
            if (buffer == null) throw new ArgumentNullException("buffer");

            if (0 < buffer.Length)
            {
                stream.BeginWrite(buffer, 0, buffer.Length, SendCallback, buffer);
            }
        }

        public void Close()
        {
            if (closed) return;
            closed = true;

            try
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
            }
            catch { }
            try
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
            catch { }

            byte[][] buffer = null;
            lock (notSended)
            {
                buffer = notSended.ToArray();
                notSended.Clear();
            }
            reciever.OnClose(buffer);
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

                    reciever.OnRecive(buffer);

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
                lock (notSended)
                {
                    notSended.Add((byte[])ar.AsyncState);
                }
                Close();
            }
        }
    }
}
