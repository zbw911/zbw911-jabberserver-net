using System.Collections.Generic;
using System.Linq;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppComponentResult : XmppHandlerResult
    {
        private readonly IList<XmppHandlerResult> results;


        public XmppComponentResult(params XmppHandlerResult[] results)
            : base(XmppSession.Empty)
        {
            Args.NotNull(results, "results");

            this.results = results.ToList();
        }

        public XmppHandlerResult Add(XmppHandlerResult result)
        {
            results.Add(result);
            return this;
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
