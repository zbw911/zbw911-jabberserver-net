using System;
using System.Xml.Linq;
using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        public XmppHandlerResult HandleXmppElement(IXmppConnection connection, XObject xmpp)
        {
            return null;
        }

        public XmppHandlerResult HandleError(IXmppConnection connection, Exception error)
        {
            return null;
        }
    }
}
