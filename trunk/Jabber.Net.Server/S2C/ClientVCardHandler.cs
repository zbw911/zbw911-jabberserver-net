using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.vcard;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientVCardHandler : XmppHandler, IXmppHandler<VcardIq>
    {
        [IQType(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(VcardIq element, XmppSession session, XmppHandlerContext context)
        {
            var to = element.HasTo ? element.To : session.Jid;
            if (element.Type == IqType.get)
            {
                element.SwitchDirection();
                element.Type = IqType.result;
                element.Vcard = context.Storages.Users.GetVCard(to.User);
                if (element.Vcard == null)
                {
                    return Error(session, ErrorCondition.ItemNotFound, element);
                }
                return Send(session, element);
            }
            else
            {
                if (session.Jid != to)
                {
                    return Error(session, ErrorCondition.Forbidden, element);
                }
                element.SwitchDirection();
                element.Type = IqType.result;
                context.Storages.Users.SetVCard(to.User, element.Vcard);
                return Send(session, element);
            }
        }
    }
}
