using System;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    class XmppDefaultHandler : XmppHandlerBase,
        IXmppHandler<Element>,
        IXmppCloseHandler,
        IXmppErrorHandler
    {
        public XmppHandlerResult ProcessElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            return element is Stanza ?
                Error(session, ErrorCode.ServiceUnavailable, (Stanza)element) :
                Error(session, agsXMPP.protocol.StreamErrorCondition.UnsupportedStanzaType);
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
