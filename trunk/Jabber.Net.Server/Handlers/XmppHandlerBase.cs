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


        public XmppHandlerResult Send(XmppSession session, params Element[] elements)
        {
            return new XmppSendResult(session, elements);
        }

        public XmppHandlerResult Error(StreamErrorCondition error)
        {
            throw new JabberStreamException(error);
        }

        public XmppHandlerResult Error(FailureCondition error)
        {
            throw new JabberSaslException(error);
        }

        public XmppHandlerResult Error(ErrorCode error)
        {
            throw new JabberStanzaException(error);
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
