using Jabber.Net.Server.Handlers;
using System;
using System.Collections.Generic;

namespace Jabber.Net.Server.Connections
{
    public class XmppConnectionManager
    {
        private XmppHandlerManager handlerManager;
        private IDictionary<Guid, IXmppConnection> connections;


        public XmppConnectionManager(XmppHandlerManager handlerManager)
        {
            this.handlerManager = handlerManager;
            this.connections = new Dictionary<Guid, IXmppConnection>(1000);
        }


        public void AddConnection(IXmppConnection connection)
        {
            lock (connection)
            {
                connections[connection.Id] = connection;
            }
            connection.BeginRecieve(handlerManager);
        }
    }
}
