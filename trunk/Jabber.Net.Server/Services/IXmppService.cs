using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Services
{
    public interface IXmppService
    {
        string Jid
        {
            get;
        }

        void Register(XmppHandlerManager handlerManager);

        void Unregister(XmppHandlerManager handlerManager);
    }
}
