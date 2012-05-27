using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection
    {
        private readonly List<byte[]> notsended = new List<byte[]>(5);
        private volatile bool closed = false;
        private TcpClient client;
        private IXmppReciever reciever;


        public TcpXmppConnection(TcpClient tcpClient)
        {
            Args.NotNull(tcpClient, "tcpClient");

            client = tcpClient;
        }


        public void BeginRecieve(IXmppReciever reciever)
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

            if (client != null)
            {
                client.Close();
                client = null;
            }

            byte[][] buffer = null;
            lock (notsended)
            {
                buffer = notsended.ToArray();
                notsended.Clear();
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
            catch (Exception error)
            {
                Log.Error(error);
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
            catch (Exception error)
            {
                Log.Error(error);
                lock (notsended)
                {
                    notsended.Add(buffer);
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
