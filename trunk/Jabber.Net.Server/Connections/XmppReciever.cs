using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(connection != null, "connection");
            Contract.Requires<ArgumentNullException>(connectionManager != null, "connectionManager");
            Contract.Requires<ArgumentNullException>(handlerManager != null, "handlerManager");

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
