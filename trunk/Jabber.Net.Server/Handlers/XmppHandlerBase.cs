using System;
using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerBase
    {
        private static readonly IUniqueId id = new IncrementalUniqueId();


        public XmppHandlerResult Send(params Element[] elements)
        {
            return Send(null, elements);
        }

        public XmppHandlerResult Send(bool offline, params Element[] elements)
        {
            return Send(null, offline, elements);
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
            return Error(null, error);
        }

        public XmppHandlerResult Error(FailureCondition error)
        {
            return Error(null, error);
        }

        public XmppHandlerResult Error(ErrorCode error)
        {
            return Error(null, error);
        }

        public XmppHandlerResult Error(Exception error)
        {
            return Error(error, null);
        }

        public XmppHandlerResult Error(XmppSession session, StreamErrorCondition error)
        {
            throw new JabberStreamException(error, session);
        }

        public XmppHandlerResult Error(XmppSession session, FailureCondition error)
        {
            throw new JabberSaslException(error, session);
        }

        public XmppHandlerResult Error(XmppSession session, ErrorCode error)
        {
            throw new JabberStanzaException(error, session);
        }

        public XmppHandlerResult Error(Exception error, XmppSession session)
        {
            return new XmppErrorResult(session, error);
        }

        public XmppHandlerResult Void()
        {
            return new XmppVoidResult();
        }


        protected string CreateId()
        {
            return id.CreateId();
        }
    }
}
