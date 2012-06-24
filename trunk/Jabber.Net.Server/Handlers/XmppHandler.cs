using System;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandler
    {
        private static readonly IUniqueId id = new RandomUniqueId();


        protected XmppHandlerResult Send(XmppSession session, params Element[] elements)
        {
            return Send(session, false, elements);
        }

        protected XmppHandlerResult Send(XmppSession session, bool offline, params Element[] elements)
        {
            return new XmppSendResult(session, offline, elements);
        }


        protected XmppHandlerResult Error(XmppSession session, StreamErrorCondition error)
        {
            return Error(session, new JabberStreamException(error));
        }

        protected XmppHandlerResult Error(XmppSession session, FailureCondition error)
        {
            return Error(session, new JabberSaslException(error));
        }

        protected XmppHandlerResult Error(XmppSession session, ErrorCondition error, Stanza stanza)
        {
            return Error(session, new JabberStanzaException(error, stanza));
        }

        protected XmppHandlerResult Error(XmppSession session, Exception error)
        {
            return new XmppErrorResult(session, error);
        }


        protected XmppHandlerResult Close(XmppSession session)
        {
            return new XmppCloseResult(session);
        }

        protected XmppHandlerResult Component(params XmppHandlerResult[] results)
        {
            return new XmppComponentResult(results);
        }

        protected XmppHandlerResult Void()
        {
            return null;
        }

        protected XmppHandlerResult Request(XmppHandlerResult request, TimeSpan timeout, XmppHandlerResult defaultResponce)
        {
            throw new NotImplementedException();
        }


        protected string CreateId()
        {
            return id.CreateId();
        }


        private class XmppCloseResult : XmppHandlerResult
        {
            public XmppCloseResult(XmppSession session)
                : base(session)
            {
            }


            public override void Execute(XmppHandlerContext context)
            {
                context.Sessions.CloseSession(Session.Id);
            }
        }
    }
}
