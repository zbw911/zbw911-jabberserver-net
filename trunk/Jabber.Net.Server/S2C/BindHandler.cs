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
                return Error(ErrorCode.BadRequest);
            }

            var answer = new IQ(IqType.result);
            answer.Id = iq.Id;

            var bind = (Bind)iq.Bind;
            var resource = !string.IsNullOrEmpty(bind.Resource) ? bind.Resource : stream.User;

            if (bind.TagName.Equals("bind", StringComparison.OrdinalIgnoreCase))
            {
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
            }
            else if (bind.TagName.Equals("unbind", StringComparison.OrdinalIgnoreCase))
            {
                if (!stream.Resources.Contains(resource)) return XmppStanzaError.ToNotFound(iq);

                context.SessionManager.CloseSession(iq.From);
                stream.UnbindResource(resource);
                if (stream.Resources.Count == 0)
                {
                    context.Sender.CloseStream(stream);
                }
            }
            else
            {
                return XmppStanzaError.ToBadRequest(iq);
            }
            if (stream.MultipleResources) answer.To = iq.From;
            return answer;
        }
    }
}
