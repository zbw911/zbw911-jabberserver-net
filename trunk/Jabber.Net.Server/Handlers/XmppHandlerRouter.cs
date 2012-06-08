using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using agsXMPP;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    class XmppHandlerRouter
    {
        private readonly IUniqueId uniqueId = new IncrementalUniqueId();
        private readonly Dictionary<Type, MethodInfo> registers = new Dictionary<Type, MethodInfo>();
        private readonly Dictionary<string, List<IInvoker>> invokers = new Dictionary<string, List<IInvoker>>(50);
        private readonly MethodInfo registerMethod;
        private readonly Dictionary<string, IXmppCloseHandler> closers = new Dictionary<string, IXmppCloseHandler>(50);
        private readonly Dictionary<string, IXmppErrorHandler> errors = new Dictionary<string, IXmppErrorHandler>(50);


        public XmppHandlerRouter()
        {
            registerMethod = GetType().GetMethod("RegisterHandlerInternal", BindingFlags.Instance | BindingFlags.NonPublic);
        }


        public string RegisterHandler(Jid jid, object handler)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(handler, "handler");

            var id = uniqueId.CreateId();
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
                    register.Invoke(this, new object[] { jid, Delegate.CreateDelegate(register.GetParameters()[1].ParameterType, handler, m), id });
                }
            }

            RegisterHandlerInternal(id, handler);

            return id;
        }

        public string RegisterHandler<T>(Jid jid, Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler) where T : Element
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(handler, "handler");

            var id = uniqueId.CreateId();
            RegisterHandlerInternal<T>(jid, handler, id);
            return id;
        }

        public string RegisterHandler(object handler)
        {
            Args.NotNull(handler, "handler");

            var id = uniqueId.CreateId();
            RegisterHandlerInternal(id, handler);
            return id;
        }

        public void UnregisterHandler(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                lock (invokers)
                {
                    foreach (var list in invokers.Values)
                    {
                        list.RemoveAll(i => i.Id == id);
                    }
                    foreach (var pair in new Dictionary<string, List<IInvoker>>(invokers))
                    {
                        if (pair.Value.Count == 0)
                        {
                            invokers.Remove(pair.Key);
                        }
                    }
                }
                lock (closers)
                {
                    closers.Remove(id);
                }
                lock (errors)
                {
                    errors.Remove(id);
                }
            }
        }

        public IEnumerable<IInvoker> GetElementHandlers(Element e, Jid j)
        {
            Args.NotNull(j, "j");
            Args.NotNull(e, "e");

            var key = GetKey(e.GetType(), j);
            lock (invokers)
            {
                List<IInvoker> list;
                return invokers.TryGetValue(key, out list) ? list.ToArray() : new IInvoker[0];
            }
        }

        public IEnumerable<IXmppCloseHandler> GetCloseHandlers()
        {
            lock (closers)
            {
                return closers.Values;
            }
        }

        public IEnumerable<IXmppErrorHandler> GetErrorHandlers()
        {
            lock (errors)
            {
                return errors.Values;
            }
        }


        private void RegisterHandlerInternal<T>(Jid jid, Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler, string id) where T : Element
        {
            var key = GetKey(typeof(T), jid);
            lock (invokers)
            {
                if (!invokers.ContainsKey(key))
                {
                    invokers[key] = new List<IInvoker>();
                }
                invokers[key].Add(new Invoker<T>(handler, id));
            }
        }

        private void RegisterHandlerInternal(string id, object handler)
        {
            if (handler is IXmppCloseHandler)
            {
                lock (closers)
                {
                    closers[id] = (IXmppCloseHandler)handler;
                }
            }
            if (handler is IXmppErrorHandler)
            {
                lock (errors)
                {
                    errors[id] = (IXmppErrorHandler)handler;
                }
            }
        }

        private string GetKey(Type type, Jid jid)
        {
            return type.FullName + jid.ToString();
        }


        public interface IInvoker
        {
            string Id { get; }

            XmppHandlerResult ProcessElement(Element e, XmppSession s, XmppHandlerContext c);
        }

        private class Invoker<T> : IInvoker where T : Element
        {
            private readonly Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler;

            public string Id { get; private set; }


            public Invoker(Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler, string id)
            {
                this.handler = handler;
                Id = id;
            }

            public XmppHandlerResult ProcessElement(Element e, XmppSession s, XmppHandlerContext c)
            {
                return handler((T)e, s, c);
            }
        }
    }
}
