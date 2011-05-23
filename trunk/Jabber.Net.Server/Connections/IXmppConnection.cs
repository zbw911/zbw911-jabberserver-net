using Jabber.Net.Server.Handlers;
using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection : IDisposable
    {
        Guid Id
        {
            get;
        }

        void BeginRecieve(XmppHandlerManager handlerManager);
    }
}
