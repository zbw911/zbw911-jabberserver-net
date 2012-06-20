using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public interface IXmppValidator<T> where T : Element
    {
        XmppHandlerResult ValidateElement(T element, XmppSession session, XmppHandlerContext context);
    }
}
