using System;
using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.bosh;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class BoshHandler : XmppHandler, IXmppHandler<Body>
    {
        public XmppHandlerResult ProcessElement(Body element, XmppSession session, XmppHandlerContext context)
        {
            if (string.IsNullOrEmpty(element.Sid))
            {
                return StartBoshSession(element, session, context);
            }

            var connection = session.Connection;
            session = context.Sessions.GetSession(element.Sid);
            var aggregator = ((BoshXmppAggregator)session.Connection);
            aggregator.AddConnection(element.Rid, connection);

            if (element.XmppRestart)
            {
                return RestartBoshSession(element, session, context);
            }

            return Void();
        }


        private XmppHandlerResult StartBoshSession(Body element, XmppSession session, XmppHandlerContext context)
        {
            var stream = new Stream
            {
                Prefix = agsXMPP.Uri.PREFIX,
                DefaultNamespace = agsXMPP.Uri.CLIENT,
                Version = element.XmppVersion,
                To = element.To,
                From = element.From,
                Language = element.GetAttribute("xml:lang"),
            };

            element.Sid = session.Id;
            element.From = element.To;
            element.To = null;
            element.SetAttribute("xmlns:xmpp", "urn:xmpp:xbosh");
            element.SetAttribute("xmlns:stream", agsXMPP.Uri.STREAM);

            var aggregator = new BoshXmppAggregator(
                session.Id,
                TimeSpan.FromSeconds(element.Wait),
                TimeSpan.FromSeconds(element.Inactivity),
                TimeSpan.FromSeconds(5))
                .AddConnection(element.Rid, session.Connection);
            aggregator.BeginReceive(context.Handlers);
            session = new XmppSession(aggregator);

            return Component(Send(session, element), Process(session, stream), Send(session, new BodyEnd()));
        }

        private XmppHandlerResult RestartBoshSession(Body element, XmppSession session, XmppHandlerContext context)
        {
            var stream = new Stream
            {
                Prefix = agsXMPP.Uri.PREFIX,
                DefaultNamespace = agsXMPP.Uri.CLIENT,
                To = element.To,
                From = element.From,
            };

            element = new Body();
            element.SetAttribute("xmlns:stream", agsXMPP.Uri.STREAM);

            return Component(Send(session, element), Process(session, stream), Send(session, new BodyEnd()));
        }
    }
}
