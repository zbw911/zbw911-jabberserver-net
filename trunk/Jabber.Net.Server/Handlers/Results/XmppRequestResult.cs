using System;
using agsXMPP.protocol.client;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppRequestResult : XmppHandlerResult
    {
        private readonly IQ iq;
        private readonly XmppHandlerResult timeoutResponce;
        private readonly TimeSpan timeout;


        public XmppRequestResult(XmppSession session, IQ iq, XmppHandlerResult timeoutResponce, TimeSpan timeout)
            : base(session)
        {
            Args.NotNull(iq, "iq");
            Args.Requires<InvalidOperationException>(timeoutResponce == null && (iq.Type == IqType.get || iq.Type == IqType.set), "timeoutResponce can not be null at get or set iq");

            this.iq = iq;
            this.timeoutResponce = timeoutResponce;
            this.timeout = timeout;
        }


        public override void Execute(XmppHandlerContext context)
        {
            if (iq.Type == IqType.get || iq.Type == IqType.set)
            {
                TaskQueue.AddTask(iq.Id, () => context.Handlers.ProcessResult(timeoutResponce));
            }
            else
            {
                TaskQueue.RemoveTask(iq.Id);
            }

            context.Handlers.ProcessResult(new XmppSendResult(Session, iq, false));
        }
    }
}
