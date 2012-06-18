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


        public XmppHandlerResult Send(XmppSession session, params Element[] elements)
        {
            return Send(session, false, elements);
        }

        public XmppHandlerResult Send(XmppSession session, bool offline, params Element[] elements)
        {
            return new XmppSendResult(session, offline, elements);
        }


        public XmppHandlerResult Error(XmppSession session, StreamErrorCondition error)
        {
            return Error(session, new JabberStreamException(error));
        }

        public XmppHandlerResult Error(XmppSession session, FailureCondition error)
        {
            return Error(session, new JabberSaslException(error));
        }

        public XmppHandlerResult Error(XmppSession session, ErrorCode error, Stanza stanza)
        {
            return Error(session, new JabberStanzaException(error, stanza));
        }

        public XmppHandlerResult Error(XmppSession session, Exception error)
        {
            return new XmppErrorResult(session, error);
        }

        
        public XmppHandlerResult Close(XmppSession session)
        {
            return new XmppCloseResult(session);
        }

        public XmppHandlerResult Component(params XmppHandlerResult[] results)
        {
            return new XmppComponentResult(results);
        }


        protected string CreateId()
        {
            return id.CreateId();
        }
    }
}
