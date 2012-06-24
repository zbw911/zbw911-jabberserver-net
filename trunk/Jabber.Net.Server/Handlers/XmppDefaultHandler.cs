using System;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using agsXMPP.protocol;

namespace Jabber.Net.Server.Handlers
{
    class XmppDefaultHandler : XmppHandler,
        IXmppHandler<Element>,
        IXmppCloseHandler,
        IXmppErrorHandler
    {
        public XmppHandlerResult ProcessElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            var stanza = element as Stanza;
            if (stanza == null)
            {
                return Error(session, StreamErrorCondition.UnsupportedStanzaType);
            }

            if (!stanza.HasTo || stanza.To.IsServer)
            {
                var iq = stanza as IQ;
                if (iq != null && (iq.Type == IqType.get || iq.Type == IqType.set))
                {
                    return Error(session, ErrorCondition.ServiceUnavailable, stanza);
                }
                else
                {
                    return Void();
                }
            }

            if (stanza.HasTo && stanza.To.IsFull)
            {
                var toSession = context.Sessions.FindSession(stanza.To);
                if (toSession == null)
                {
                    return Error(session, ErrorCondition.RecipientUnavailable, stanza);
                }

                var iq = stanza as IQ;
                if (iq != null && (iq.Type == IqType.get || iq.Type == IqType.set))
                {
                    return Request(Send(toSession, stanza), TimeSpan.FromSeconds(5), Error(session, ErrorCondition.RecipientUnavailable, stanza));
                }
                else
                {
                    return Send(toSession, stanza);
                }
            }

            return Void();
        }

        public XmppHandlerResult OnClose(XmppSession session, XmppHandlerContext context)
        {
            return Close(session);
        }

        public XmppHandlerResult OnError(Exception error, XmppSession session, XmppHandlerContext context)
        {
            return Error(session, error);
        }
    }
}
