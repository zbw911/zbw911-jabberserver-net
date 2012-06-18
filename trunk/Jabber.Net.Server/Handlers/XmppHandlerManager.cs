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
        private readonly IXmppResolver resolver;
        private readonly XmppHandlerContext context;


        public XmppHandlerManager(XmppSessionManager sessionManager, IXmppResolver resolver)
        {
            Args.NotNull(sessionManager, "sessionManager");
            Args.NotNull(resolver, "resolver");

            this.sessionManager = sessionManager;
            this.resolver = resolver;
            this.context = new XmppHandlerContext(this, resolver);

            RegisterHandler(new Jid("{user}@{server}/{resource}"), new XmppValidationHandler());
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

        public string RegisterHandler(object handler)
        {
            ProcessRegisterHandler(handler as IXmppRegisterHandler);
            return router.RegisterHandler(handler);
        }

        public void UnregisterHandler(string id)
        {
            router.UnregisterHandler(id);
        }


        public void ProcessXmppElement(IXmppEndPoint endpoint, Element element)
        {
            try
            {
                Args.NotNull(endpoint, "endpoint");
                Args.NotNull(element, "element");

                var session = GetSession(endpoint);
                var to = element.GetAttribute("to");
                var jid = !string.IsNullOrEmpty(to) ? new Jid(to) : session.Jid ?? Jid.Empty;

                foreach (var handler in router.GetElementHandlers(element, jid))
                {
                    var result = handler.ProcessElement(element, session, context);
                    ProcessResult(result);
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
                Args.NotNull(endpoint, "endpoint");

                var session = GetSession(endpoint);
                try
                {
                    foreach (var handler in router.GetCloseHandlers())
                    {
                        var result = handler.OnClose(session, context);
                        ProcessResult(result);
                    }
                }
                finally
                {
                    ProcessResult(new XmppCloseResult(session));
                }
            }
            catch (Exception error)
            {
                ProcessError(endpoint, error);
            }
        }

        public void ProcessError(IXmppEndPoint endpoint, Exception error)
        {
            try
            {
                Args.NotNull(endpoint, "endpoint");
                Args.NotNull(error, "error");

                var session = GetSession(endpoint);
                try
                {
                    foreach (var handler in router.GetErrorHandlers())
                    {
                        var result = handler.OnError(error, session, context);
                        ProcessResult(result);
                    }
                }
                finally
                {
                    ProcessResult(new XmppErrorResult(session, error));
                }
            }
            catch (Exception innererror)
            {
                Log.Error(innererror);
            }
        }

        public void ProcessResult(XmppHandlerResult result)
        {
            Args.NotNull(result, "result");

            result.Execute(context);
        }


        private void ProcessRegisterHandler(IXmppRegisterHandler handler)
        {
            if (handler != null)
            {
                handler.OnRegister(context);
            }
        }

        private XmppSession GetSession(IXmppEndPoint endpoint)
        {
            return sessionManager.GetSession(endpoint.SessionId) ?? new XmppSession(endpoint);
        }
    }
}
