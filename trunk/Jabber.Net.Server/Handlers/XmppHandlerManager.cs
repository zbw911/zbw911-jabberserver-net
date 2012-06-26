using System;
using System.Linq;
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
        private readonly XmppDefaultHandler defaultHandler;


        public XmppHandlerManager(XmppSessionManager sessionManager, IXmppResolver resolver)
        {
            Args.NotNull(sessionManager, "sessionManager");
            Args.NotNull(resolver, "resolver");

            this.sessionManager = sessionManager;
            this.resolver = resolver;
            this.context = new XmppHandlerContext(this, resolver);
            this.defaultHandler = new XmppDefaultHandler();
        }


        public string RegisterHandler(Jid jid, object handler)
        {
            ProcessRegisterHandler(handler as IXmppRegisterHandler);
            return router.RegisterHandler(jid, handler);
        }

        public string RegisterHandler(object handler)
        {
            return RegisterHandler(new Jid("{user}@{server}/{resource}"), handler);
        }

        public void UnregisterHandler(string id)
        {
            router.UnregisterHandler(id);
        }


        public void ProcessXmppElement(IXmppEndPoint endpoint, Element element)
        {
            try
            {
                Args.NotNull(element, "element");

                var session = GetSession(endpoint);
                var to = element.GetAttribute("to");
                var jid = !string.IsNullOrEmpty(to) ? new Jid(to) : session.Jid ?? Jid.Empty;

                var invokers = router.GetElementHandlers(element, jid);
                var handlers = invokers.Where(i => i.MethodInfo.Name == "ProcessElement");
                var validators = invokers.Where(i => i.MethodInfo.Name == "ValidateElement");

                foreach (var validator in validators)
                {
                    var result = validator.ProcessElement(element, session, context);
                    if (result != null)
                    {
                        ProcessResult(result);
                        return;
                    }
                }

                if (handlers.Any())
                {
                    foreach (var handler in handlers)
                    {
                        XmppHandlerResult result;
                        foreach (var validator in handler.MethodInfo.GetCustomAttributes(false).OfType<XmppValidationAttribute>())
                        {
                            result = validator.ValidateElement(element, session, context);
                            if (result != null)
                            {
                                ProcessResult(result);
                                continue;
                            }
                        }
                        result = handler.ProcessElement(element, session, context);
                        ProcessResult(result);
                    }
                }
                else
                {
                    ProcessResult(defaultHandler.ProcessElement(element, session, context));
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
                    ProcessResult(defaultHandler.OnClose(session, context));
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
                    ProcessResult(defaultHandler.OnError(error, session, context));
                }
            }
            catch (Exception innererror)
            {
                Log.Error(innererror);
            }
        }

        public void ProcessResult(XmppHandlerResult result)
        {
            if (result != null)
            {
                result.Execute(context);
            }
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
            Args.NotNull(endpoint, "endpoint");
            return sessionManager.GetSession(endpoint.SessionId) ?? new XmppSession(endpoint);
        }
    }
}
