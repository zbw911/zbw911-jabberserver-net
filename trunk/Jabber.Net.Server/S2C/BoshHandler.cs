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
        public TimeSpan WaitTimeout
        {
            get;
            set;
        }

        public TimeSpan InactivityTimeout
        {
            get;
            set;
        }

        public TimeSpan SendTimeout
        {
            get;
            set;
        }


        public BoshHandler()
        {
            WaitTimeout = TimeSpan.FromMinutes(1);
            InactivityTimeout = TimeSpan.FromMinutes(2);
            SendTimeout = TimeSpan.FromSeconds(5);
        }


        public XmppHandlerResult ProcessElement(Body element, XmppSession session, XmppHandlerContext context)
        {
            XmppSession realSession;
            if (string.IsNullOrEmpty(element.Sid))
            {
                element.Wait = element.Wait == 0 || WaitTimeout.TotalSeconds < element.Wait ? (int)WaitTimeout.TotalSeconds : element.Wait;
                element.Inactivity = element.Inactivity == 0 || InactivityTimeout.TotalSeconds < element.Inactivity ? (int)InactivityTimeout.TotalSeconds : element.Inactivity;

                var aggregator = new BoshXmppAggregator(
                        session.Id,
                        TimeSpan.FromSeconds(element.Wait),
                        TimeSpan.FromSeconds(element.Inactivity),
                        SendTimeout);
                aggregator.BeginReceive(context.Handlers);
                realSession = new XmppSession(aggregator);
            }
            else
            {
                realSession = context.Sessions.GetSession(element.Sid);
            }

            if (realSession == null)
            {
                return Error(session, agsXMPP.protocol.StreamErrorCondition.ImproperAddressing);
            }

            ((BoshXmppAggregator)realSession.Connection).AddConnection(element.Rid, session.Connection);

            if (string.IsNullOrEmpty(element.Sid))
            {
                return StartBoshSession(element, realSession, context);
            }
            else if (element.XmppRestart)
            {
                return RestartBoshSession(element, realSession, context);
            }

            return Void();
        }


        private XmppHandlerResult StartBoshSession(Body element, XmppSession session, XmppHandlerContext context)
        {
            var body = new Body
            {
                XmppVersion = "1.0",
                Sid = session.Id,
                From = element.To,
                Secure = false,
                Inactivity = element.Inactivity,
                Wait = element.Wait,
            };
            body.SetAttribute("xmlns:xmpp", "urn:xmpp:xbosh");
            body.SetAttribute("xmpp:restartlogic", true);

            var stream = new Stream
            {
                Prefix = agsXMPP.Uri.PREFIX,
                DefaultNamespace = agsXMPP.Uri.CLIENT,
                Version = element.XmppVersion,
                To = element.To,
                Language = element.GetAttribute("xml:lang"),
            };

            return Component(Send(session, body), Process(session, stream), Send(session, new BodyEnd()));
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

            return Process(session, stream);
        }
    }
}
