using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServerDiscoInfoHandler : XmppHandler, IXmppHandler<DiscoInfoIq>
    {
        private readonly DiscoInfoIq iq;


        public ServerDiscoInfoHandler(string[] identities, string[] features)
        {
            Args.NotNull(identities, "identities");
            Args.NotNull(features, "features");

            iq = new DiscoInfoIq(IqType.result);

            foreach (var identity in identities)
            {
                var a = identity.Split('/');
                iq.Query.AddIdentity(new DiscoIdentity(a.ElementAtOrDefault(1), a.ElementAtOrDefault(2), a.ElementAtOrDefault(0)));
            }
            foreach (var feature in features)
            {
                iq.Query.AddFeature(new DiscoFeature(feature));
            }
        }


        public XmppHandlerResult ProcessElement(DiscoInfoIq element, XmppSession session, XmppHandlerContext context)
        {
            var result = (DiscoInfoIq)iq.Clone();
            result.Id = element.Id;
            result.From = element.To;

            return Send(session, result);
        }
    }
}
