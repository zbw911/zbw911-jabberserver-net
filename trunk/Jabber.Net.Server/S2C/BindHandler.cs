using System;
using agsXMPP.protocol.iq.bind;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using agsXMPP.protocol.client;

namespace Jabber.Net.Server.S2C
{
    class BindHandler : XmppHandlerBase, IXmppHandler<BindIq>, IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Sessions.SupportBind = true;
        }

        public XmppHandlerResult ProcessElement(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            if (element.Type != IqType.set)
            {
                return Error(ErrorCode.BadRequest, element);
            }
            else if (element.Query.TagName.Equals("bind", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessBind(element, session, context);
            }
            else if (element.Query.TagName.Equals("unbind", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessUnbind(element, session, context);
            }
            else
            {
                return Error(ErrorCode.BadRequest, element);
            }
        }

        private XmppHandlerResult ProcessBind(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            /*
            var answer = new IQ(IqType.result);
            answer.Id = iq.Id;

            var bind = (Bind)iq.Bind;
            var resource = !string.IsNullOrEmpty(bind.Resource) ? bind.Resource : stream.User;
            if (stream.MultipleResources) answer.To = iq.From;
            return answer;

            var jid = new Jid(stream.User, stream.Domain, resource);

                var findedSession = context.Sessions.FindSession(jid);
                if (findedSession != null)
                {
                    if (session.Id != findedSession.Id)
                    {
                        context.Sender.SendToAndClose(session.Stream, XmppStreamError.Conflict);
                    }
                    else
                    {
                        return XmppStanzaError.ToConflict(iq);
                    }
                }

                stream.BindResource(resource);
                context.SessionManager.AddSession(new XmppSession(jid, stream));
                answer.Bind = new Bind(jid);
             
             */
            throw new NotImplementedException();
        }

        private XmppHandlerResult ProcessUnbind(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            var resource = ((Bind)element.Query).Resource;
            return session.Jid.Resource == resource ? Close(session) : Error(ErrorCode.NotFound, element);
        }
    }
}
