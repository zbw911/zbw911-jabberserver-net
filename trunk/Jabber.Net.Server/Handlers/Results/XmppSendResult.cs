using System;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppSendResult : XmppHandlerResult
    {
        private readonly bool offline;

        internal Element Element
        {
            get;
            private set;
        }


        public XmppSendResult(XmppSession session, Element element, bool offline)
            : base(session)
        {
            Args.NotNull(element, "element");

            this.Element = element;
            this.offline = offline;
        }

        public override void Execute(XmppHandlerContext context)
        {
            Session.EndPoint.Send(Element, offline ? notsended => Save(notsended, context) : (Action<Element>)null);
        }

        private void Save(Element e, XmppHandlerContext context)
        {
            context.Storages.Elements.SaveElements(Session.Jid, "offline", e);
        }
    }
}
