using System;
using System.Collections.Generic;
using System.Linq;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    class XmppReciever : IXmppReciever
    {
        private readonly IXmppConnection connection;
        private readonly XmppHandlerManager handlerManager;
        private readonly XmppConnectionManager connectionManager;
        private readonly XmppStreamParser parser;


        public XmppReciever(IXmppConnection connection, XmppConnectionManager connectionManager, XmppHandlerManager handlerManager)
        {
            Args.NotNull(connection, "connection");
            Args.NotNull(connectionManager, "connectionManager");
            Args.NotNull(handlerManager, "handlerManager");

            this.connection = connection;
            this.connectionManager = connectionManager;
            this.handlerManager = handlerManager;
            this.parser = new XmppStreamParser();

            parser.Parsed += Parsed;
            parser.Error += Error;
        }


        public void OnRecive(byte[] buffer)
        {
            parser.ParseAsync(buffer);
        }

        public void OnClose(IEnumerable<byte[]> notSended)
        {
            try
            {
                handlerManager.ProcessClose(connection, notSended.Select(bytes => parser.Parse(bytes)));
            }
            finally
            {
                connectionManager.CloseConnection(connection.Id);
            }
        }

        private void Parsed(object sender, XmppStreamParser.ParsedArgs e)
        {
            handlerManager.ProcessElement(connection, e.XmppElement);
        }

        private void Error(object sender, XmppStreamParser.ParseErrorArgs e)
        {
            handlerManager.ProcessError(connection, e.Error);
        }
    }
}
