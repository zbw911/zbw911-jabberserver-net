using System;
using System.Collections.Generic;
using agsXMPP;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Xmpp;

namespace Jabber.Net.Server.Handlers
{
    class XmppHandlerRouter
    {
        public void RegisterHandler(Jid jid, IXmppHandler handler)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(handler, "handler");
        }


        public IEnumerable<IInvoker> GetHandlers(XmppElement e, Jid j)
        {
            Args.NotNull(j, "j");
            Args.NotNull(e, "e");

            return null;
        }


        public interface IInvoker
        {
            XmppHandlerResult ProcessElement(XmppElement e, XmppSession s, XmppHandlerContext c);
        }

        private class Invoker<T> : IInvoker where T : Element
        {
            private readonly Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler;


            public Invoker(Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler)
            {
                this.handler = handler;
            }

            public XmppHandlerResult ProcessElement(XmppElement e, XmppSession s, XmppHandlerContext c)
            {
                return handler((T)e.Element, s, c);
            }
        }
    }
}
