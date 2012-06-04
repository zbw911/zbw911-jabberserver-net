using System;
using System.Collections.Generic;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Connections;
using Jabber.Net.Xmpp;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        private readonly IDictionary<Type, IList<IInvoker>> streamHandlers = new Dictionary<Type, IList<IInvoker>>();


        public void RegisterStreamHandler<T>(Func<T, XmppHandlerContext, XmppHandlerResult> handler) where T : Element        
        {
            Args.NotNull(handler, "handler");

            var key = typeof(T);
            var list = streamHandlers.ContainsKey(key) ? streamHandlers[key] : new List<IInvoker>();
            list.Add(new Invoker<T>(handler));
            streamHandlers[key] = list;
        }


        public void ProcessXmppElement(IXmppEndPoint endpoint, XmppElement e)
        {
            try
            {
                IList<IInvoker> invokers;
                if (streamHandlers.TryGetValue(e.Node.GetType(), out invokers))
                {
                    foreach (var invoker in invokers)
                    {
                        var result = invoker.Invoke((XmppElement)e.Clone(), new XmppHandlerContext());
                    }
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


        private interface IInvoker
        {
            XmppHandlerResult Invoke(XmppElement e, XmppHandlerContext context);
        }

        private class Invoker<T> : IInvoker where T : Element
        {
            private readonly Func<T, XmppHandlerContext, XmppHandlerResult> handler;


            public Invoker(Func<T, XmppHandlerContext, XmppHandlerResult> handler)
            {
                this.handler = handler;
            }

            public XmppHandlerResult Invoke(XmppElement e, XmppHandlerContext context)
            {
                return handler((T)e.Node, context);
            }
        }
    }
}
