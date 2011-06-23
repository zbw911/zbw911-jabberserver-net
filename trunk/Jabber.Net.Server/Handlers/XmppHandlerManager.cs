using System;
using Jabber.Net.Server.Connections;
using System.Collections.Generic;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        public void ProcessElement(IXmppConnection connection, XmppElement e)
        {
            
        }

        public void ProcessError(IXmppConnection connection, Exception error)
        {
            
        }

        public void ProcessClose(IXmppConnection connection, IEnumerable<XmppElement> notSended)
        {
            
        }
    }
}
