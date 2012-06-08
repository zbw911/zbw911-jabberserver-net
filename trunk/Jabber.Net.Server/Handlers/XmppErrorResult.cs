using System;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppErrorResult : XmppHandlerResult
    {
        private readonly Exception error;
        private readonly XmppSession session;


        public XmppErrorResult(Exception error, XmppSession session)
        {
            Args.NotNull(error, "error");
            Args.NotNull(session, "session");

            this.error = error;
            this.session = session;
        }


        public override void Execute(XmppHandlerContext context)
        {
            Args.NotNull(context, "context");

            if (error is JabberException)
            {
                session.EndPoint.Send(((JabberException)error).ToElement(), null);
            }
            if (error is JabberStreamException)
            {
                context.SessionManager.CloseSession(session.Id);
            }
            /*if (ex is JabberException)
            {
                log.Warn("JabberError", ex);
                var je = (JabberException)ex;
                var error = je.ToElement();

                if (je.StreamError)
                {
                    ((Error)error).Text = je.Message;
                    sender.SendTo(stream, error);
                }
                else
                {
                    if (node is Stanza && error is StanzaError)
                    {
                        sender.SendTo(stream, XmppStanzaError.ToErrorStanza((Stanza)node, (StanzaError)error));
                    }
                    else
                    {
                        var streamError = XmppStreamError.InternalServerError;
                        streamError.Text = "Stanza error in stream.";
                        sender.SendToAndClose(stream, streamError);
                    }
                }

                if (je.CloseStream) sender.CloseStream(stream);
            }
            else
            {
                log.Error("InternalServerError", ex);
                var error = XmppStreamError.InternalServerError;
                error.Text = ex.Message;
                sender.SendToAndClose(stream, error);
            }*/
        }
    }
}
