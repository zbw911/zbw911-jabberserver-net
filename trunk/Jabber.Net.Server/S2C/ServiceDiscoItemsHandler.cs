using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceDiscoItemsHandler : XmppHandler, IXmppHandler<DiscoItemsIq>
    {
        private readonly DiscoItemsIq iq;


        public ServiceDiscoItemsHandler(ServiceInfo[] items)
        {
            Args.NotNull(items, "items");

            iq = new DiscoItemsIq(IqType.result);
            foreach (var item in items)
            {
                iq.Query.AddDiscoItem(item.Jid, item.Name);
            }
        }


        [IQType(IqType.get)]
        public XmppHandlerResult ProcessElement(DiscoItemsIq element, XmppSession session, XmppHandlerContext context)
        {
            var result = (DiscoItemsIq)iq.Clone();
            result.Id = element.Id;
            result.To = session.Jid;
            result.From = element.To;
            return Send(session, result);
        }
    }
}
