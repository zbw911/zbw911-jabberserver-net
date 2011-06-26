using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Services.Jabber
{
    class ClientStreamHandler : IXmppStreamHandler
    {
        public string Namespace
        {
            get { return "http://etherx.jabber.org/streams"; }
        }

        public void ProcessElement(IXmppConnection connection, XmppElement e)
        {
            throw new NotImplementedException();
        }

        public void ProcessClose(IXmppConnection connection, IEnumerable<XmppElement> notSended)
        {
            throw new NotImplementedException();
        }
    }
}
