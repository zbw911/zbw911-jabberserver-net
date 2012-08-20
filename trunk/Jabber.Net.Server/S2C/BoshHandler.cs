using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.bosh;
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
            else
            {
                session = context.Sessions.GetSession(element.Sid);
                ((BoshXmppAggregator)session.Connection).AddConnection(element.Rid, session.Connection);

                var body = new Body { };
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

            var aggregator = new BoshXmppAggregator(
                session.Id,
                TimeSpan.FromSeconds(element.Wait),
                TimeSpan.FromSeconds(element.Inactivity),
                TimeSpan.FromMilliseconds(100))
                .AddConnection(element.Rid, session.Connection);
            session = new XmppSession(aggregator);

            return Component(Send(session, element), Process(session, stream), Send(session, new BodyEnd()));
        }
    }
}
