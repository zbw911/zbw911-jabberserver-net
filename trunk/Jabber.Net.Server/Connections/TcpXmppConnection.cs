using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection
    {
        private readonly object locker = new object();
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
            stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, new AsyncState(stream, buffer, null));
        }

        public void Send(byte[] buffer, Action<byte[]> error)
        {
            Args.NotNull(buffer, "buffer");

            if (0 < buffer.Length)
            {
                try
                {
                    var stream = client.GetStream();
                    stream.BeginWrite(buffer, 0, buffer.Length, SendCallback, new AsyncState(stream, buffer, error));
                }
                catch (Exception ex)
                {
                    if (!IgnoreError(ex))
                    {
                        Log.Error(ex);
                    }
                    try
                    {
                        if (error != null)
                        {
                            error(buffer);
                        }
                    }
                    finally
                    {
                        Close();
                    }
                }
            }
        }

        public void Close()
        {
            lock (locker)
            {
                if (closed) return;
                closed = true;

                try
                {
                    client.GetStream().Close();
                }
                catch (Exception) { }
                try
                {
                    client.Close();
                }
                catch (Exception) { }
                try
                {
                    reciever.OnClose();
                }
                catch (Exception) { }
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
                if (!IgnoreError(error))
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
            var onerror = state.Error;

            try
            {
                stream.EndWrite(ar);
            }
            catch (Exception error)
            {
                if (!IgnoreError(error))
                {
                    Log.Error(error);
                }
                try
                {
                    if (onerror != null)
                    {
                        onerror(buffer);
                    }
                }
                finally
                {
                    Close();
                }
            }
        }

        private void RequiresNotClosed()
        {
            Args.Requires<ObjectDisposedException>(!closed, GetType().FullName);
        }

        private bool IgnoreError(Exception error)
        {
            return error is ObjectDisposedException || error is IOException;
        }


        private class AsyncState
        {
            public readonly Stream Stream;
            public readonly byte[] Buffer;
            public readonly Action<byte[]> Error;


            public AsyncState(Stream stream, byte[] buffer, Action<byte[]> error)
            {
                Stream = stream;
                Buffer = buffer;
                Error = error;
            }
        }
    }
}
