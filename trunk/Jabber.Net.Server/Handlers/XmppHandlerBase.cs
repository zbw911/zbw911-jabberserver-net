using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerBase
    {
        public XmppHandlerResult Send(XmppSession session, params Element[] elements)
        {
            return new XmppSendResult(session, elements);
        }
    }
}
