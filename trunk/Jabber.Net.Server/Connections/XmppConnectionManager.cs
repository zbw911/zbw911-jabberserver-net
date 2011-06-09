using System;
using System.Collections.Generic;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    public class XmppConnectionManager
    {
        private readonly XmppHandlerManager handlerManager;
        private readonly IDictionary<Guid, IXmppConnection> connections;


        public XmppConnectionManager(XmppHandlerManager handlerManager)
        {
            this.connections = new Dictionary<Guid, IXmppConnection>(1000);
            this.handlerManager = handlerManager;
        }


        public void AddConnection(IXmppConnection connection)
        {
            lock (connections)
            {
                connections.Add(connection.Id, connection);
            }
            connection.StartRecieve(new XmppReciever(connection, handlerManager));
        }

        public void CloseConnection(Guid connectionId)
        {
            IXmppConnection connection;
            lock (connections)
            {
                connections.TryGetValue(connectionId, out connection);
            }
            if (connection != null)
            {
                connection.Close();
                lock (connections)
                {
                    connections.Remove(connectionId);
                }
            }
        }
    }
}
