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
    class BindHandler : XmppHandler, IXmppHandler<BindIq>, IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Sessions.SupportBind = true;
        }

        [IQType(ErrorCondition.BadRequest, IqType.set)]
        public XmppHandlerResult ProcessElement(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            if (element.Query.TagName.Equals("bind", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessBind(element, session, context);
            }
            else if (element.Query.TagName.Equals("unbind", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessUnbind(element, session, context);
            }
            else
            {
                return Error(session, ErrorCondition.BadRequest, element);
            }
        }

        private XmppHandlerResult ProcessBind(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            if (session.Binded)
            {
                return Error(session, ErrorCondition.Conflict, element);
            }

            var resource = ((Bind)element.Query).Resource;
            session.BindResource(!string.IsNullOrEmpty(resource) ? resource : session.Jid.User);

            var answer = new BindIq(IqType.result) { Id = element.Id, Query = new Bind(session.Jid) };
            var send = Send(session, new BindIq(IqType.result) { Id = element.Id, Query = new Bind(session.Jid) });

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
            return session.Jid.Resource == resource ? Close(session) : Error(session, ErrorCondition.ItemNotFound, element);
        }
    }
}
