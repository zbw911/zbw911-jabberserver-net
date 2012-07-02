using System.Collections.Generic;
using System.Linq;
using Jabber.Net.Server.Sessions;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppProcessResult : XmppHandlerResult
    {
        private readonly Element element;


        public XmppProcessResult(XmppSession session, Element element)
            : base(session)
        {
            Args.NotNull(element, "element");
            this.element = element;
        }

        public override void Execute(XmppHandlerContext context)
        {
            context.Handlers.ProcessElement(Session.EndPoint, element);
        }
    }
}
