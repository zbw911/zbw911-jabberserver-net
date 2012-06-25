using System;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers.Results;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandler
    {
        private static readonly IUniqueId id = new RandomUniqueId();


        protected XmppHandlerResult Send(XmppSession session, Element element)
        {
            return Send(session, element, false);
        }

        protected XmppHandlerResult Send(XmppSession session, Element element, bool offline)
        {
            return new XmppSendResult(session, element, offline);
        }


        protected XmppHandlerResult Error(XmppSession session, StreamErrorCondition error)
        {
            return Error(session, new JabberStreamException(error));
        }

        protected XmppHandlerResult Error(XmppSession session, FailureCondition error)
        {
            return Error(session, new JabberSaslException(error));
        }

        protected XmppHandlerResult Error(XmppSession session, ErrorCondition error, Stanza stanza)
        {
            return Error(session, new JabberStanzaException(error, stanza));
        }

        protected XmppHandlerResult Error(XmppSession session, Exception error)
        {
            Args.NotNull(error, "error");

            var je = (error as JabberException) ?? new JabberStreamException(error);
            var send = Send(session, je.ToElement());
            return je.CloseStream ? Component(send, Close(session)) : send;
        }


        protected XmppCloseResult Close(XmppSession session)
        {
            return new XmppCloseResult(session);
        }

        protected XmppComponentResult Component(params XmppHandlerResult[] results)
        {
            return new XmppComponentResult(results);
        }

        protected XmppHandlerResult Void()
        {
            return null;
        }

        protected XmppRequestResult Request(XmppHandlerResult request, TimeSpan timeout, XmppHandlerResult timeoutResponce)
        {
            return new XmppRequestResult(request, timeout, timeoutResponce);
        }


        protected string CreateId()
        {
            return id.CreateId();
        }
    }
}
