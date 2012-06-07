using System;
using agsXMPP.protocol.sasl;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class AuthDigestMD5Handler : XmppHandlerBase,
        IXmppHandler<Auth>,
        IXmppHandler<Response>,
        IXmppHandler<Abort>,
        IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.SessionManager.SupportedAuthMechanisms.Add(new AuthMechanism(MechanismType.DIGEST_MD5, true));
        }


        public XmppHandlerResult ProcessElement(Auth element, XmppSession session, XmppHandlerContext context)
        {
            throw new NotImplementedException();
        }

        public XmppHandlerResult ProcessElement(Response element, XmppSession session, XmppHandlerContext context)
        {
            throw new NotImplementedException();
        }

        public XmppHandlerResult ProcessElement(Abort element, XmppSession session, XmppHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
