using System;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

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
                // server answer
                var iq = stanza as IQ;
                if (iq != null && (iq.Type == IqType.get || iq.Type == IqType.set))
                {
                    // unknown request iq
                    return Error(session, ErrorCondition.ServiceUnavailable, stanza);
                }
                return Void();
            }

            if (stanza.HasTo && stanza.To.IsFull)
            {
                // route stanza to client
                var to = context.Sessions.FindSession(stanza.To);
                if (to == null)
                {
                    return Error(session, ErrorCondition.RecipientUnavailable, stanza);
                }

                var iq = stanza as IQ;
                if (iq != null)
                {
                    if (iq.Type == IqType.get || iq.Type == IqType.set)
                    {
                        return Request(to, iq, Error(session, ErrorCondition.RecipientUnavailable, stanza));
                    }
                    else
                    {
                        return RequestCancel(to, iq);
                    }
                }
                return Send(to, stanza);
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
