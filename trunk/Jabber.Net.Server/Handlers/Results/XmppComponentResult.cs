using System.Collections.Generic;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppComponentResult : XmppHandlerResult
    {
        private readonly List<XmppHandlerResult> results = new List<XmppHandlerResult>();


        public XmppComponentResult(params XmppHandlerResult[] results)
            : base(XmppSession.Empty)
        {
            Args.NotNull(results, "results");

            this.results.AddRange(results);
        }


        public override void Execute(XmppHandlerContext context)
        {
            foreach (var r in results)
            {
                context.Handlers.ProcessResult(r);
            }
        }
    }
}
