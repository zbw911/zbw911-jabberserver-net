using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection
    {
        private readonly object locker = new object();
        private readonly List<byte[]> notsended = new List<byte[]>(5);
        private readonly TcpClient client;
        private bool closed = false;
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
            lock (locker)
            {
                if (closed) return;
                closed = true;

                client.Close();
                reciever.OnClose(notsended.ToArray());
            }
        }


        private void ReadCallback(IAsyncResult ar)
        {
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
                if (!(error is ObjectDisposedException))
                {
                    Log.Error(error);
                }
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
                if (!(error is ObjectDisposedException))
                {
                    Log.Error(error);
                }
                if (Monitor.TryEnter(locker))
                {
                    try
                    {
                        notsended.Add(buffer);
                    }
                    finally
                    {
                        Monitor.Exit(locker);
                    }
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
