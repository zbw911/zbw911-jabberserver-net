using System;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppRequestResult : XmppHandlerResult
    {
        private readonly XmppHandlerResult request;
        private readonly TimeSpan timeout;
        private readonly XmppHandlerResult timeoutResponce;


        public XmppRequestResult(XmppHandlerResult request, TimeSpan timeout, XmppHandlerResult timeoutResponce)
            : base(XmppSession.Empty)
        {
            Args.NotNull(request, "request");
            Args.NotNull(timeoutResponce, "timeoutResponce");

            this.request = request;
            this.timeout = timeout;
            this.timeoutResponce = timeoutResponce;
        }


        public override void Execute(XmppHandlerContext context)
        {
            request.Execute(context);
            if (request is XmppSendResult)
            {
                var e = ((XmppSendResult)request).Element;
                if (e.TagName == "iq")
                {
                    var type = e.GetAttribute("type");
                    if (type == "get" || type == "set")
                    {
                        TaskQueue.AddTask(e.GetAttribute("id"), () => timeoutResponce.Execute(context));
                    }
                }
            }
        }
    }
}
