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
            this.handlerManager = handlerManager;
            this.connections = new Dictionary<Guid, IXmppConnection>(1000);
        }


        public void AddConnection(IXmppConnection connection)
        {
            lock (connection)
            {
                connections.Add(connection.Id, connection);
            }
            connection.Closed += ConnectionClosed;
            connection.Recieved += ConnectionRecieved;
            connection.StartRecieve();
        }
        

        private void ConnectionClosed(object sender, XmppConnectionCloseArgs e)
        {
            var connection = (IXmppConnection)sender;
            try
            {
                connection.Closed -= ConnectionClosed;
                connection.Recieved -= ConnectionRecieved;
            }
            finally
            {
                lock (connections)
                {
                    connections.Remove(connection.Id);
                }
            }
        }

        private void ConnectionRecieved(object sender, XmppConnectionRecieveArgs e)
        {

        }
    }
}
