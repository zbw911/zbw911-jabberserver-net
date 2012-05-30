using System.Collections.Generic;
using System.Linq;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Connections
{
    class XmppConnection : IXmppReciever, IXmppSender
    {
        private readonly IXmppConnection connection;
        private readonly XmppHandlerManager handlerManager;
        private readonly XmppStreamParser parser;


        public string SessionId
        {
            get;
            set;
        }


        public XmppConnection(IXmppConnection connection, XmppHandlerManager handlerManager)
        {
            Args.NotNull(connection, "connection");
            Args.NotNull(handlerManager, "handlerManager");

            SessionId = IdGenerator.NewId();
            this.connection = connection;
            this.handlerManager = handlerManager;
            this.parser = new XmppStreamParser();

            parser.Parsed += Parsed;
            parser.Error += Error;
        }

        public void BeginReceive()
        {
            connection.BeginRecieve(this);
        }

        public void Send(XmppElement e)
        {
            connection.Send(parser.ToBytes(e));
        }

        public void SendAndClose(XmppElement e)
        {
            Send(e);
            Close();
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
            handlerManager.ProcessClose(this, notsended.Select(e => parser.Parse(e)));
        }
        
        private void Parsed(object sender, XmppStreamParser.ParsedArgs e)
        {
            handlerManager.ProcessElement(this, e.XmppElement);
        }

        private void Error(object sender, XmppStreamParser.ParseErrorArgs e)
        {
            handlerManager.ProcessError(this, e.Error);
        }
    }
}
