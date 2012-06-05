using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public interface IXmppHandler
    {
        void Register(XmppHandlerManager handlerManager);
    }
}
