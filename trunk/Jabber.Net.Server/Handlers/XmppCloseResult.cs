using System;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppCloseResult : XmppHandlerResult
    {
        public XmppCloseResult(XmppSession session)
            : base(session)
        {
        }


        public override void Execute(XmppResultContext context)
        {
            context.Sessions.CloseSession(context.Session.Id);
        }
    }
}
