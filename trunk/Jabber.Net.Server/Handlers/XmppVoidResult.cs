using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppVoidResult : XmppHandlerResult
    {
        public XmppVoidResult()
            : base(XmppSession.Empty)
        {
        }


        public override void Execute(XmppHandlerContext context)
        {
        }
    }
}
