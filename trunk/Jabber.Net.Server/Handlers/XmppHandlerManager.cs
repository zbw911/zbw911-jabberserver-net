using System;
using agsXMPP;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        private readonly XmppHandlerRouter router = new XmppHandlerRouter();
        private readonly XmppSessionManager sessionManager;


        public XmppHandlerManager(XmppSessionManager sessionManager)
        {
            Args.NotNull(sessionManager, "sessionManager");

            this.sessionManager = sessionManager;
        }


        public string RegisterHandler(Jid jid, object handler)
        {
            ProcessRegisterHandler(handler as IXmppRegisterHandler);
            return router.RegisterHandler(jid, handler);
        }

        public string RegisterHandler<T>(Jid jid, Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler) where T : Element
        {
            return router.RegisterHandler<T>(jid, handler);
        }

        public string RegisterHandler(IXmppErrorHandler handler)
        {
            ProcessRegisterHandler(handler as IXmppRegisterHandler);
            return router.RegisterHandler(handler);
        }

        public string RegisterHandler(IXmppCloseHandler handler)
        {
            ProcessRegisterHandler(handler as IXmppRegisterHandler);
            return router.RegisterHandler(handler);
        }

        public void UnregisterHandler(string id)
        {
            router.UnregisterHandler(id);
        }

        public void ProcessXmppElement(IXmppEndPoint endpoint, Element e)
        {
            try
            {
                var jid = new Jid(e.GetAttribute("to") ?? string.Empty);
                foreach (var handler in router.GetElementHandlers(e, jid))
                {
                    var result = handler.ProcessElement(e, GetSession(endpoint), GetContext());
                    ProcessResult(endpoint, result);
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
                foreach (var handler in router.GetCloseHandlers())
                {
                    var result = handler.OnClose(GetSession(endpoint), GetContext());
                    ProcessResult(endpoint, result);
                }
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
                foreach (var handler in router.GetErrorHandlers())
                {
                    var result = handler.OnError(error, GetSession(endpoint), GetContext());
                    ProcessResult(endpoint, result);
                }
            }
            catch (Exception innererror)
            {
                Log.Error(innererror);
            }
        }

        public void ProcessResult(IXmppEndPoint endpoint, XmppHandlerResult result)
        {

        }


        private void ProcessRegisterHandler(IXmppRegisterHandler handler)
        {
            if (handler != null)
            {
                handler.OnRegister(GetContext());
            }
        }

        private XmppHandlerContext GetContext()
        {
            return new XmppHandlerContext();
        }

        private XmppSession GetSession(IXmppEndPoint endpoint)
        {
            return sessionManager.GetSession(endpoint.SessionId) ?? new XmppSession(endpoint);
        }
    }
}
