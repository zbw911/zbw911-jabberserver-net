using System;
using System.Collections.Generic;
using Jabber.Net.Server.Connections;
using agsXMPP.protocol.client;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        public void ProcessElement(IXmppConnection connection, XmppElement e)
        {
            if (e.Node is IQ || e.Node is Message || e.Node is Presence)
            {
                ProcessStanza(connection, e);
            }
            else
            {

            }
        }

        public void ProcessError(IXmppConnection connection, Exception error)
        {

        }

        public void ProcessClose(IXmppConnection connection, IEnumerable<XmppElement> notSended)
        {

        }


        private void ProcessStanza(IXmppConnection connection, XmppElement e)
        {

        }
    }
}
