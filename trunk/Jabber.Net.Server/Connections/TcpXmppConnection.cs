using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection
    {
        public Guid Id
        {
            get;
            private set;
        }


        public TcpXmppConnection(TcpClient tcpClient)
        {
            Id = Guid.NewGuid();
        }

        public void BeginRecieve(XmppHandlerManager handlerManager)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {

        }

        void IDisposable.Dispose()
        {
            Close();
        }
    }
}
