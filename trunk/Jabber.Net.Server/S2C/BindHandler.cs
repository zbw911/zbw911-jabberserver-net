using System;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.register;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class BindHandler : XmppHandlerBase, IXmppHandler<BindIq>, IXmppHandler<Stanza>, IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Sessions.SupportBind = true;
        }

        public XmppHandlerResult ProcessElement(Stanza element, XmppSession session, XmppHandlerContext context)
        {
            if (!session.Binded && !(element is BindIq) && !(element is RegisterIq))
            {
                return Error(StreamErrorCondition.NotAuthorized);
            }
            return Void();
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
            if (session.Binded)
            {
                return Error(ErrorCode.Conflict, element);
            }

            var resource = ((Bind)element.Query).Resource;
            session.BindResource(!string.IsNullOrEmpty(resource) ? resource : session.Jid.User);

            var answer = new BindIq(IqType.result) { Id = element.Id, Query = new Bind(session.Jid) };
            var send = Send(new BindIq(IqType.result) { Id = element.Id, Query = new Bind(session.Jid) });

            var conflict = context.Sessions.FindSession(session.Jid);
            if (conflict != null && !session.Equals(conflict))
            {
                return Component(send, Close(conflict));
            }
            else
            {
                return send;
            }
        }

        private XmppHandlerResult ProcessUnbind(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            var resource = ((Bind)element.Query).Resource;
            return session.Jid.Resource == resource ? Close(session) : Error(ErrorCode.NotFound, element);
        }
    }
}
