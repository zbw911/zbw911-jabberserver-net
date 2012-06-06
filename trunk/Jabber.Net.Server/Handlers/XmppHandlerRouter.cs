using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using agsXMPP;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Xmpp;

namespace Jabber.Net.Server.Handlers
{
    class XmppHandlerRouter
    {
        private readonly MethodInfo registerMethod;
        private readonly IDictionary<Type, MethodInfo> registers = new Dictionary<Type, MethodInfo>();
        private readonly IDictionary<Type, IInvoker> tests = new Dictionary<Type, IInvoker>();


        public XmppHandlerRouter()
        {
            registerMethod = GetType().GetMethod("RegisterHandlerGeneric");
        }


        public void RegisterHandler(Jid jid, object handler)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(handler, "handler");

            foreach (var m in handler.GetType().GetMethods().Where(m => m.Name == "ProcessElement"))
            {
                var parameters = m.GetParameters();
                if (parameters.Length == 3 &&
                    parameters[0].ParameterType.IsSubclassOf(typeof(Element)) &&
                    parameters[1].ParameterType == typeof(XmppSession) &&
                    parameters[2].ParameterType == typeof(XmppHandlerContext))
                {
                    var elementType = parameters[0].ParameterType;
                    MethodInfo register;
                    lock (registers)
                    {
                        if (!registers.ContainsKey(elementType))
                        {
                            registers[elementType] = registerMethod.MakeGenericMethod(elementType);
                        }
                        register = registers[elementType];
                    }
                    register.Invoke(this, new object[] { jid, Delegate.CreateDelegate(register.GetParameters()[1].ParameterType, handler, m) });
                }
            }
        }

        public void RegisterHandlerGeneric<T>(Jid jid, Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler) where T : Element
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(handler, "handler");

            tests[typeof(T)] = new Invoker<T>(handler);
        }
        

        public IEnumerable<IInvoker> GetHandlers(XmppElement e, Jid j)
        {
            Args.NotNull(j, "j");
            Args.NotNull(e, "e");

            return new[] { tests[e.Element.GetType()] };
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
