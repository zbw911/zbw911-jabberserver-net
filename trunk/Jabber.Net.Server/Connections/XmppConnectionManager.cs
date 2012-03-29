using System;
using System.Collections.Generic;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    public class XmppConnectionManager
    {
        private readonly XmppHandlerManager handlerManager;
        private readonly IDictionary<string, IXmppConnection> connections;


        public XmppConnectionManager(XmppHandlerManager handlerManager)
        {
            this.connections = new Dictionary<string, IXmppConnection>(1000);
            this.handlerManager = handlerManager;
        }


        public void AddConnection(IXmppConnection connection)
        {
            lock (connections)
            {
                connections.Add(connection.Id, connection);
            }

            var reciever = new XmppReciever(connection, this, handlerManager);
            connection.Recieve(reciever);
        }

        public void CloseConnection(string connectionId)
        {
            IXmppConnection connection;
            lock (connections)
            {
                connections.TryGetValue(connectionId, out connection);
            }
            if (connection != null)
            {
                try
                {
                    connection.Close();
                }
                finally
                {
                    lock (connections)
                    {
                        connections.Remove(connectionId);
                    }
                }
            }
        }
    }
}
