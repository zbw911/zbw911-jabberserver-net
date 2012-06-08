using System;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppErrorResult : XmppHandlerResult
    {
        private readonly JabberException error;


        public XmppErrorResult(XmppSession session, Exception error)
            : base(session)
        {
            Args.NotNull(error, "error");

            this.error = (error as JabberException) ?? new JabberStreamException(error);
        }


        public override void Execute(XmppResultContext context)
        {
            context.Session.EndPoint.Send(error.ToElement(), null);
            if (error.CloseStream)
            {
                context.SessionManager.CloseSession(context.Session.Id);
            }
        }
    }
}
