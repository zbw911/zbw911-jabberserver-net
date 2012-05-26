using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection
    {
        private readonly List<byte[]> notSended = new List<byte[]>(5);
        private volatile bool closed = false;
        private TcpClient client;
        private IXmppReciever reciever;


        public string Id
        {
            get;
            private set;
        }


        public TcpXmppConnection(TcpClient tcpClient)
        {
            Args.NotNull(tcpClient, "tcpClient");

            Id = Guid.NewGuid().ToString("N");
            client = tcpClient;
        }


        public void Recieve(IXmppReciever reciever)
        {
            RequiresNotClosed();
            Args.NotNull(reciever, "reciever");

            this.reciever = reciever;

            var stream = client.GetStream();
            var buffer = new byte[client.ReceiveBufferSize];
            stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, new AsyncState(stream, buffer));
        }

        public void Send(byte[] buffer)
        {
            RequiresNotClosed();
            Args.NotNull(buffer, "buffer");

            var stream = client.GetStream();
            stream.BeginWrite(buffer, 0, buffer.Length, SendCallback, new AsyncState(stream, buffer));
        }

        public void Close()
        {
            if (closed) return;
            closed = true;

            try
            {
                if (client != null)
                {
                    client.Close();
                    client = null;
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
            reciever = null;
        }


        private void ReadCallback(IAsyncResult ar)
        {
            if (closed) return;

            var state = (AsyncState)ar.AsyncState;
            var stream = state.Stream;
            var buffer = state.Buffer;
            
            try
            {
                var readed = stream.EndRead(ar);
                if (0 < readed)
                {
                    reciever.OnRecive(buffer.Take(readed).ToArray());
                    stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, state);
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
            var state = (AsyncState)ar.AsyncState;
            var stream = state.Stream;
            var buffer = state.Buffer;

            try
            {
                stream.EndWrite(ar);
            }
            catch (Exception)
            {
                lock (notSended)
                {
                    notSended.Add(buffer);
                }
                Close();
            }
        }

        private void RequiresNotClosed()
        {
            Args.Requires<ObjectDisposedException>(!closed, GetType().FullName);
        }


        private class AsyncState
        {
            public readonly Stream Stream;
            public readonly byte[] Buffer;


            public AsyncState(Stream stream, byte[] buffer)
            {
                Stream = stream;
                Buffer = buffer;
            }
        }
    }
}
