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


        public Guid Id
        {
            get;
            private set;
        }
        

        public TcpXmppConnection(TcpClient tcpClient)
        {
            Id = Guid.NewGuid();

            this.tcpClient = tcpClient;
            this.stream = tcpClient.GetStream();

            this.readBuffer = new byte[tcpClient.ReceiveBufferSize];
            this.notSended = new List<byte[]>(5);
        }

        public void StartRecieve(IXmppReciever reciever)
        {
            if (closed) throw new ObjectDisposedException(GetType().FullName);

            this.reciever = reciever;
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
                tcpClient = null;
            }
            catch { }
            try
            {
                stream.Close();
                stream = null;
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
