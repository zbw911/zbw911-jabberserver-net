using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.vcard;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceVCardHandler : XmppHandler, IXmppHandler<VcardIq>
    {
        private readonly VcardIq iq;


        public ServiceVCardHandler(ServiceInfo serviceInfo)
        {
            Args.NotNull(serviceInfo, "serviceInfo");

            iq = new VcardIq(IqType.result, new Vcard { Fullname = serviceInfo.Name, Description = serviceInfo.Copyrigth, Url = serviceInfo.Url });
        }


        [IQType(IqType.get)]
        public XmppHandlerResult ProcessElement(VcardIq element, XmppSession session, XmppHandlerContext context)
        {
            var result = (VcardIq)iq.Clone();
            result.Id = element.Id;
            result.To = session.Jid;
            result.From = element.To;
            return Send(session, result);
        }
    }
}
