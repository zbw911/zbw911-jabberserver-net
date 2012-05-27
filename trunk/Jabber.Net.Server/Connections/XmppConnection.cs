using System;
using System.Collections.Generic;
using Jabber.Net.Server.Utils;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    class XmppConnection : IXmppReciever
    {
        private readonly IXmppConnection connection;
        private readonly XmppConnectionManager connectionManager;
        private readonly XmppHandlerManager handlerManager;
        private readonly XmppStreamParser parser;


        public string Id
        {
            get;
            private set;
        }


        public XmppConnection(IXmppConnection connection, XmppConnectionManager connectionManager, XmppHandlerManager handlerManager)
        {
            Args.NotNull(connection, "connection");
            Args.NotNull(connectionManager, "connectionManager");
            Args.NotNull(handlerManager, "handlerManager");

            Id = IdGenerator.NewId();
            this.connection = connection;
            this.connectionManager = connectionManager;
            this.handlerManager = handlerManager;
            this.parser = new XmppStreamParser();

            parser.Parsed += Parsed;
            parser.Error += Error;
        }

        public void Close()
        {
            connection.Close();
        }


        void IXmppReciever.OnRecive(byte[] buffer)
        {
            parser.ParseAsync(buffer);
        }

        void IXmppReciever.OnClose(IEnumerable<byte[]> notsended)
        {
            connectionManager.CloseConnection(Id);
        }


        private void Parsed(object sender, XmppStreamParser.ParsedArgs e)
        {
            //handlerManager.ProcessElement(connection, e.XmppElement);
        }

        private void Error(object sender, XmppStreamParser.ParseErrorArgs e)
        {
            //handlerManager.ProcessError(connection, e.Error);
        }
    }
}
