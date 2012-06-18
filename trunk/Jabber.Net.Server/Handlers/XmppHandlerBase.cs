using System;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerBase
    {
        private static readonly IUniqueId id = new RandomUniqueId();


        public XmppHandlerResult Send(params Element[] elements)
        {
            return Send(XmppSession.Current, elements);
        }

        public XmppHandlerResult Send(bool offline, params Element[] elements)
        {
            return Send(XmppSession.Current, offline, elements);
        }

        public XmppHandlerResult Send(XmppSession session, params Element[] elements)
        {
            return Send(session, false, elements);
        }

        public XmppHandlerResult Send(XmppSession session, bool offline, params Element[] elements)
        {
            return new XmppSendResult(session, offline, elements);
        }

        public XmppHandlerResult Error(StreamErrorCondition error)
        {
            return Error(XmppSession.Current, error);
        }

        public XmppHandlerResult Error(FailureCondition error)
        {
            return Error(XmppSession.Current, error);
        }

        public XmppHandlerResult Error(ErrorCode error, Stanza stanza)
        {
            return Error(XmppSession.Current, error, stanza);
        }

        public XmppHandlerResult Error(Exception error)
        {
            return Error(error, XmppSession.Current);
        }

        public XmppHandlerResult Error(XmppSession session, StreamErrorCondition error)
        {
            return Error(new JabberStreamException(error), session);
        }

        public XmppHandlerResult Error(XmppSession session, FailureCondition error)
        {
            return Error(new JabberSaslException(error), session);
        }

        public XmppHandlerResult Error(XmppSession session, ErrorCode error, Stanza stanza)
        {
            return Error(new JabberStanzaException(error, stanza), session);
        }

        public XmppHandlerResult Error(Exception error, XmppSession session)
        {
            return new XmppErrorResult(session, error);
        }

        public XmppHandlerResult Void()
        {
            return new XmppVoidResult();
        }

        public XmppHandlerResult Close(XmppSession session)
        {
            return new XmppCloseResult(session);
        }


        protected string CreateId()
        {
            return id.CreateId();
        }
    }
}
