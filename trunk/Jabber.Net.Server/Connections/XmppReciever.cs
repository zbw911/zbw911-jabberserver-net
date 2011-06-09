using System;
using System.Collections.Generic;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    class XmppReciever : IXmppReciever
    {
        private readonly IXmppConnection connection;
        private readonly XmppHandlerManager handlerManager;
        private readonly XmppStreamParser parser;


        public XmppReciever(IXmppConnection connection, XmppHandlerManager handlerManager)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (handlerManager == null) throw new ArgumentNullException("handlerManager");

            this.connection = connection;
            this.handlerManager = handlerManager;
            this.parser = new XmppStreamParser();

            parser.Parsed += Parsed;
            parser.Error += Error;
        }


        public void OnRecive(byte[] buffer)
        {
            parser.Parse(buffer);
        }

        public void OnClose(IEnumerable<byte[]> notSended)
        {
            
        }


        private void Parsed(object sender, XmppStreamParsedArgs e)
        {
            handlerManager.HandleXmppElement(connection.Id, e.Xmpp);
        }

        private void Error(object sender, XmppStreamParseErrorArgs e)
        {
            handlerManager.HandleError(connection.Id, e.Error);
        }
    }
}
