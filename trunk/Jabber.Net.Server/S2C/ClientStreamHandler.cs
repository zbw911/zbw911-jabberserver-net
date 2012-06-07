using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.sasl;
using agsXMPP.protocol.stream;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.S2C
{
    class ClientStreamHandler : XmppHandlerBase, IXmppHandler<Stream>
    {
        private readonly IUniqueId id = new IncrementalUniqueId();


        public XmppHandlerResult ProcessElement(Stream element, XmppSession session, XmppHandlerContext context)
        {
            var stream = new Stream();
            stream.Version = "1.0";
            stream.From = element.To;
            stream.Id = id.CreateId();
            stream.Prefix = Uri.PREFIX;

            stream.Features = new Features();
            stream.Features.Prefix = agsXMPP.Uri.PREFIX;
            if (session.Authenticated)
            {
                stream.Features.AddChild(new Bind());
                stream.Features.AddChild(new Session());
            }
            else
            {
                stream.Features.Mechanisms = new Mechanisms();
                foreach (var m in context.SessionManager.SupportedAuthMechanisms)
                {
                    stream.Features.Mechanisms.AddChild(new Mechanism(m.Name));
                    if (m.Required)
                    {
                        stream.Features.Mechanisms.AddChild(new Element("required"));
                    }
                }
            }

            session.Id = id.CreateId();
            session.EndPoint.SessionId = session.Id;
            context.SessionManager.OpenSession(session);

            return Send(session, stream);
        }
    }
}
