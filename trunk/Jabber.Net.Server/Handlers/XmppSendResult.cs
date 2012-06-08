using System;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppSendResult : XmppHandlerResult
    {
        private readonly XmppSession session;
        private readonly bool offline;
        private readonly Element[] elements;

        public XmppSendResult(XmppSession session, bool offline, params Element[] elements)
        {
            Args.NotNull(session, "session");
            Args.NotNull(elements, "elements");
            Args.Requires<ArgumentOutOfRangeException>(0 < elements.Length, "Elements list is empty.");

            this.session = session;
            this.offline = offline;
            this.elements = elements;
        }

        public override void Execute(XmppHandlerContext context)
        {
            Args.NotNull(context, "context");

            foreach (var e in elements)
            {
                session.EndPoint.Send(e, offline ? notsended => Save(notsended, context) : (Action<Element>)null);
            }
        }

        private void Save(Element e, XmppHandlerContext context)
        {
            //context.Storage<IOfflineStorage>().Save(e);
        }
    }
}
