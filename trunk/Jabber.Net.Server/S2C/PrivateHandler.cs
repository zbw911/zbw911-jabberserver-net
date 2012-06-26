using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.@private;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PrivateHandler : XmppHandler,
        IXmppHandler<PrivateIq>
    {
        [IQType(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(PrivateIq element, XmppSession session, XmppHandlerContext context)
        {
            if (element.HasTo && element.To != session.Jid)
            {
                return Error(session, ErrorCondition.Forbidden, element);
            }
            if (!element.HasChildElements)
            {
                return Error(session, ErrorCondition.BadRequest, element);
            }

            if (element.Type == IqType.get)
            {
                var result = new List<Element>();
                foreach (var e in element.Query.ChildNodes.OfType<Element>().ToArray())
                {
                    var restored = context.Storages.Elements.GetSingleElement(session.Jid, e.TagName, e.Namespace);
                    if (restored != null)
                    {
                        result.Add(restored);
                    }
                }
                element.Query.RemoveAllChildNodes();
                foreach (var e in result)
                {
                    element.Query.AddChild(e);
                }
            }
            else if (element.Type == IqType.set)
            {
                foreach (var e in element.Query.ChildNodes.OfType<Element>())
                {
                    context.Storages.Elements.SaveSingleElement(session.Jid, e);
                }
                element.Query.RemoveAllChildNodes();
            }

            element.Type = IqType.result;
            element.SwitchDirection();
            return Send(session, element);
        }
    }
}
