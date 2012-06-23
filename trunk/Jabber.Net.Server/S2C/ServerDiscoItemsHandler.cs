using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.sasl;
using agsXMPP.protocol.stream;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using agsXMPP.protocol.iq.disco;
using System.Collections.Generic;
using agsXMPP.protocol.client;

namespace Jabber.Net.Server.S2C
{
    class ServerDiscoItemsHandler : XmppHandler, IXmppHandler<DiscoItemsIq>
    {
        private readonly DiscoItemsIq iq;


        public ServerDiscoItemsHandler(string[] items)
        {
            iq = new DiscoItemsIq(IqType.result);
            //iq.Query.AddDiscoItem(new DiscoItem().Id
        }


        public XmppHandlerResult ProcessElement(DiscoItemsIq element, XmppSession session, XmppHandlerContext context)
        {

            return Void();
        }
    }
}
