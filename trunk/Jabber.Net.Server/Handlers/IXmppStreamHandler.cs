using System.Collections.Generic;
using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Handlers
{
    public interface IXmppStreamHandler
    {
        string Namespace
        {
            get;
        }

        void ProcessElement(IXmppConnection connection, XmppElement e);

        void ProcessClose(IXmppConnection connection, IEnumerable<XmppElement> notSended);
    }
}
