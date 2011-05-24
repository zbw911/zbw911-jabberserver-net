using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        public XmppHandlerResult HandleXmppElement(Guid connectionId, XObject xmpp)
        {
            return null;
        }

        public XmppHandlerResult HandleError(Guid connectionId, Exception error)
        {
            return null;
        }
    }
}
