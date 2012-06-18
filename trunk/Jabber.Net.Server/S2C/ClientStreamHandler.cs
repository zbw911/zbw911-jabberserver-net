using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.sasl;
using agsXMPP.protocol.stream;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientStreamHandler : XmppHandlerBase, IXmppHandler<Stream>
    {
        private readonly Jid domain;


        public ClientStreamHandler(Jid domain)
        {
            Args.NotNull(domain, "domain");
            this.domain = domain;
        }


        public XmppHandlerResult ProcessElement(Stream element, XmppSession session, XmppHandlerContext context)
        {
            if (element.To != domain)
            {
                return Error(session, StreamErrorCondition.HostUnknown);
            }

            var stream = new Stream
            {
                Id = CreateId(),
                Prefix = Uri.PREFIX,
                Version = element.Version,
                From = element.To,
                Features = new Features { Prefix = Uri.PREFIX },
                Language = element.Language,
            };

            if (!session.Authenticated)
            {
                session.Jid = stream.From;
                session.Language = stream.Language;
                context.Sessions.OpenSession(session);

                stream.Features.Mechanisms = new Mechanisms();
                foreach (var m in context.Sessions.SupportedAuthMechanisms)
                {
                    stream.Features.Mechanisms.AddChild(new Mechanism(m.Name));
                    if (m.Required)
                    {
                        stream.Features.Mechanisms.AddChild(new Element("required"));
                    }
                }
            }
            else
            {
                if (context.Sessions.SupportBind)
                {
                    stream.Features.AddChild(new Bind());
                }
                if (context.Sessions.SupportSession)
                {
                    stream.Features.AddChild(new Session());
                }
            }

            return Send(session, stream);
        }
    }
}
