using System;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

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
            Args.NotNull(context, "context");

            if (Element.TagName == "iq")
            {
                var type = Element.GetAttribute("type");
                if (type == "result" || type == "error")
                {
                    TaskQueue.RemoveTask(Element.GetAttribute("id"));
                }
            }

            Session.EndPoint.Send(Element, offline ? notsended => Save(notsended, context) : (Action<Element>)null);
        }

        private void Save(Element e, XmppHandlerContext context)
        {
            //context.Storage<IOfflineStorage>().Save(e);
        }
    }
}
