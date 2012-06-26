using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.version;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceInfoHandler : XmppHandler,
        IXmppHandler<LastIq>,
        IXmppHandler<VersionIq>
    {
        private readonly DateTime started = DateTime.UtcNow;


        [IQType(IqType.get)]
        public XmppHandlerResult ProcessElement(LastIq element, XmppSession session, XmppHandlerContext context)
        {
            element.SwitchDirection();
            element.Type = IqType.result;
            element.Query.Seconds = (int)(DateTime.UtcNow - started).TotalSeconds;
            return Send(session, element);
        }

        [IQType(IqType.get)]
        public XmppHandlerResult ProcessElement(VersionIq element, XmppSession session, XmppHandlerContext context)
        {
            element.SwitchDirection();
            element.Type = IqType.result;
            element.Query.Os = Environment.OSVersion.ToString();
            return Send(session, element);
        }
    }
}
