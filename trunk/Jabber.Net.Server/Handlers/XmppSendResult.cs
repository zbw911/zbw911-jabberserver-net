using System;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppSendResult : XmppHandlerResult
    {
        private readonly bool offline;
        private readonly Element[] elements;

        public XmppSendResult(XmppSession session, bool offline, params Element[] elements)
            : base(session)
        {
            Args.NotNull(elements, "elements");
            Args.Requires<ArgumentOutOfRangeException>(0 < elements.Length, "Elements list is empty.");

            this.offline = offline;
            this.elements = elements;
        }

        public override void Execute(XmppHandlerContext context)
        {
            Args.NotNull(context, "context");

            foreach (var e in elements)
            {
                Session.EndPoint.Send(e, offline ? notsended => Save(notsended, context) : (Action<Element>)null);
            }
        }

        private void Save(Element e, XmppHandlerContext context)
        {
            System.Threading.ThreadPool.RegisterWaitForSingleObject(
            //context.Storage<IOfflineStorage>().Save(e);
        }
    }
}
