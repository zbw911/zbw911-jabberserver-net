using System;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Utils;
using Jabber.Net.Xmpp;

namespace Jabber.Net.Server.Connections
{
    class XmppConnection : IXmppReciever, IXmppEndPoint
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

            this.connection = connection;
            this.handlerManager = handlerManager;
            this.parser = new XmppStreamParser();

            parser.Parsed += (s, e) => handlerManager.ProcessXmppElement(this, e.XmppElement);
            parser.Error += (s, e) => handlerManager.ProcessError(this, e.Error);
        }

        public void BeginReceive()
        {
            connection.BeginRecieve(this);
        }

        public void Send(XmppElement e, Action<XmppElement> error)
        {
            connection.Send(parser.ToBytes(e), _ => error(e));
        }

        public void Close()
        {
            connection.Close();
        }


        void IXmppReciever.OnRecive(byte[] buffer)
        {
            parser.ParseAsync(buffer);
        }

        void IXmppReciever.OnClose()
        {
            handlerManager.ProcessClose(this);
        }
    }
}
