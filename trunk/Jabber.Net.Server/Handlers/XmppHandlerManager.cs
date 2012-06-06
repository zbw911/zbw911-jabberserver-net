using System;
using agsXMPP;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Xmpp;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        private readonly XmppHandlerRouter router = new XmppHandlerRouter();
        private readonly XmppHandlerContext context = new XmppHandlerContext();


        public string RegisterHandler(Jid jid, object handler)
        {
            ProcessRegisterHandler(handler as IXmppRegisterHandler);
            return router.RegisterHandler(jid, handler);
        }

        public string RegisterHandler<T>(Jid jid, Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler) where T : Element
        {
            return router.RegisterHandler<T>(jid, handler);
        }

        public void UnregisterHandler(string id)
        {
            router.UnregisterHandler(id);
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


        private void ProcessRegisterHandler(IXmppRegisterHandler handler)
        {
            if (handler != null)
            {
                handler.OnRegister(context);
            }
        }
    }
}
