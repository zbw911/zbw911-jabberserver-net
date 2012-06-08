using System;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppSendResult : XmppHandlerResult
    {
        private readonly XmppSession session;
        private readonly Element[] elements;

        public XmppSendResult(XmppSession session, params Element[] elements)
        {
            Args.NotNull(session, "session");
            Args.NotNull(elements, "elements");
            Args.Requires<ArgumentOutOfRangeException>(0 < elements.Length, "Elements list is empty.");

            this.session = session;
            this.elements = elements;
        }

        public override void Execute(XmppHandlerContext context)
        {
            Args.NotNull(context, "context");

            foreach (var e in elements)
            {
                session.EndPoint.Send(e, null);
            }
        }
    }
}
