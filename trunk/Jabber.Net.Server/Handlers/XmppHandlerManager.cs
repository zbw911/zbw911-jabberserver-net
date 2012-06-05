using System;
using System.Collections.Generic;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Connections;
using Jabber.Net.Xmpp;
using Jabber.Net.Server.Sessions;
using agsXMPP;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        private readonly XmppHandlerRouter router = new XmppHandlerRouter();


        public void RegisterHandler(IXmppHandler handler)
        {
            RegisterHandler(new Jid("*"), handler);
        }

        public void RegisterHandler(Jid jid, IXmppHandler handler)
        {
            handler.Register(this);
        }

        public void RegisterHandler<T>(Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler)
        {
            //router.RegisterHandler(jid, handler);
        }

        public void ProcessXmppElement(IXmppEndPoint endpoint, XmppElement e)
        {
            try
            {
                var jid = new Jid(e.Element.GetAttribute("to") ?? string.Empty);
                foreach (var handler in router.GetHandlers(e, jid))
                {
                    var result = handler.ProcessElement(e, null, new XmppHandlerContext());
                }
            }
            catch (Exception error)
            {
                ProcessError(endpoint, error);
            }
        }

        public void ProcessClose(IXmppEndPoint endpoint)
        {
            try
            {

            }
            catch (Exception error)
            {
                Log.Error(error);
            }
        }

        public void ProcessError(IXmppEndPoint endpoint, Exception error)
        {
            try
            {

            }
            catch (Exception innererror)
            {
                Log.Error(innererror);
            }
        }
    }
}
