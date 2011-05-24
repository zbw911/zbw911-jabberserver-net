using System;
using System.Collections.Generic;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    public class XmppConnectionManager
    {
        private readonly XmppHandlerManager handlerManager;
        private readonly IDictionary<Guid, IXmppConnection> connections;
        private readonly XmppStreamParser parser;


        public XmppConnectionManager(XmppHandlerManager handlerManager)
        {
            this.connections = new Dictionary<Guid, IXmppConnection>(1000);
            this.handlerManager = handlerManager;
            this.parser = new XmppStreamParser();
            this.parser.Parsed += ParserParsed;
            this.parser.Error += ParserError;
        }


        public void AddConnection(IXmppConnection connection)
        {
            lock (connections)
            {
                connections.Add(connection.Id, connection);
            }
            connection.Closed += ConnectionClosed;
            connection.Recieved += ConnectionRecieved;
            connection.StartRecieve();
        }

        public void CloseConnection(Guid connectionId)
        {
            IXmppConnection connection;
            lock (connections)
            {
                connections.TryGetValue(connectionId, out connection);
            }
            if (connection != null) connection.Close();
        }


        private void ConnectionClosed(object sender, XmppConnectionCloseArgs e)
        {
            var connection = (IXmppConnection)sender;
            try
            {
                connection.Closed -= ConnectionClosed;
                connection.Recieved -= ConnectionRecieved;
                parser.Reset(e.ConnectionId);
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
            parser.Parse(e.ConnectionId, e.Buffer);
        }

        private void ParserParsed(object sender, XmppStreamParsedArgs e)
        {
            handlerManager.HandleXmppElement(e.ConnectionId, e.Xmpp);
        }

        private void ParserError(object sender, XmppStreamParseErrorArgs e)
        {
            handlerManager.HandleXmppElement(e.ConnectionId, e.Error);
        }
    }
}
