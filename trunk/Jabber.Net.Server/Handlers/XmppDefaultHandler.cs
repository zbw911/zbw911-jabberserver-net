using System;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.auth;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.register;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    class XmppDefaultHandler : XmppHandler,
        IXmppHandler<Element>,
        IXmppCloseHandler,
        IXmppErrorHandler
    {
        [DefaultValidator]
        public XmppHandlerResult ProcessElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            var stanza = element as Stanza;
            if (stanza == null)
            {
                return Error(session, StreamErrorCondition.UnsupportedStanzaType);
            }

            if (!stanza.HasTo || stanza.To.IsServer)
            {
                // server answer
                var iq = stanza as IQ;
                if (iq != null && (iq.Type == IqType.get || iq.Type == IqType.set))
                {
                    // unknown request iq
                    return Error(session, ErrorCondition.ServiceUnavailable, stanza);
                }
                return Void(); // ignore
            }

            if (stanza.HasTo && stanza.To.IsFull)
            {
                // route stanza to client
                var to = context.Sessions.GetSession(stanza.To);
                if (to == null)
                {
                    return Error(session, ErrorCondition.RecipientUnavailable, stanza);
                }

                var iq = stanza as IQ;
                if (iq != null)
                {
                    if (iq.Type == IqType.get || iq.Type == IqType.set)
                    {
                        var defaultResponse = Error(session, ErrorCondition.RecipientUnavailable, stanza);
                        iq.From = session.Jid;
                        return Request(to, iq, defaultResponse);
                    }
                    else
                    {
                        return Response(to, iq);
                    }
                }
                return Send(to, stanza);
            }

            return Void();
        }

        public XmppHandlerResult OnClose(XmppSession session, XmppHandlerContext context)
        {
            return Component(session.Available ? Process(session, Presence.Unavailable(session.Jid, null)) : Void(), Close(session));
        }

        public XmppHandlerResult OnError(Exception error, XmppSession session, XmppHandlerContext context)
        {
            Log.Error(error);
            return Error(session, error);
        }


        class DefaultValidator : XmppValidationAttribute
        {
            public override XmppHandlerResult ValidateElement(Element element, XmppSession session, XmppHandlerContext context)
            {
                var stanza = element as Stanza;
                if (stanza != null)
                {
                    // auhtentication
                    if (!session.Authenticated && !(stanza is AuthIq) && !(stanza is RegisterIq))
                    {
                        return Error(session, StreamErrorCondition.NotAuthorized);
                    }

                    // resource binding
                    if (!session.Binded && !(stanza is AuthIq) && !(stanza is RegisterIq) && !(stanza is BindIq))
                    {
                        return Error(session, StreamErrorCondition.NotAuthorized);
                    }

                    // correct from
                    if (stanza.HasFrom)
                    {
                        if (stanza.From.IsBare && session.Jid.BareJid != stanza.From.BareJid)
                        {
                            return Error(session, StreamErrorCondition.InvalidFrom);
                        }
                        if (stanza.From.IsFull && session.Jid != stanza.From)
                        {
                            return Error(session, StreamErrorCondition.InvalidFrom);
                        }
                    }
                    if (!stanza.HasFrom)
                    {
                        stanza.From = session.Jid;
                    }
                }
                return Success();
            }
        }
    }
}
