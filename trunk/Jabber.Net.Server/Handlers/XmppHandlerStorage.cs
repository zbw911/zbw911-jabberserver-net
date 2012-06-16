using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    class XmppHandlerStorage
    {
        private readonly ReaderWriterLock locker = new ReaderWriterLock();
        private readonly IUniqueId uniqueId = new GuidUniqueId();
        private readonly Dictionary<string, XmppHandlerRouter.IInvoker> handlers = new Dictionary<string, XmppHandlerRouter.IInvoker>();
        private readonly RouterStore<string> handlerIds = new RouterStore<string>();
        private readonly RouterStore<Type> types = new RouterStore<Type>();
        private readonly RouterStore<Jid> jids = new RouterStore<Jid>();


        public void Register(Type type, Jid jid, XmppHandlerRouter.IInvoker invoker)
        {
            using (locker.WriteLock())
            {
                var id = uniqueId.CreateId();
                while (true)
                {
                    types.Add(type, id);
                    type = type.BaseType;
                    if (type == null || type == typeof(object))
                    {
                        break;
                    }
                }
                jids.Add(jid, id);
                handlerIds.Add(invoker.HandlerId, id);
                handlers.Add(id, invoker);
            }
        }

        public void Unregister(string handlerId)
        {
            using (locker.WriteLock())
            {
                foreach (var id in handlerIds.GetIdentifiers(handlerId))
                {
                    types.Remove(id);
                    jids.Remove(id);
                    handlers.Remove(id);
                }
                handlerIds.Remove(handlerId);
            }
        }

        public IEnumerable<XmppHandlerRouter.IInvoker> GetInvokers(Type type, Jid jid)
        {
            using (locker.ReadLock())
            {
                var byType = types.GetIdentifiers(type);

                var result = Intersect(byType, jid);
                if (jid.HasResource)
                {
                    result = Intersect(byType, new Jid(jid.User, jid.Server, "{resource}"));
                }
                if (jid.HasUser)
                {
                    result = Intersect(byType, new Jid("{user}", jid.Server, jid.HasResource ? "{resource}" : string.Empty));
                }
                if (!string.IsNullOrEmpty(jid.Server))
                {
                    result = Intersect(byType, new Jid(jid.HasUser ? "{user}" : string.Empty, "{server}", jid.HasResource ? "{resource}" : string.Empty));
                }
                return result
                    .Select(id => handlers[id])
                    .ToArray();
            }
        }

        private IEnumerable<string> Intersect(IEnumerable<string> result, Jid jid)
        {
            var byJids = jids.GetIdentifiers(jid);
            return 0 < byJids.Count() ? result.Intersect(byJids) : result;
        }


        private class RouterStore<T>
        {
            private readonly Dictionary<T, List<string>> store = new Dictionary<T, List<string>>(100);

            public void Add(T value, string id)
            {
                if (!store.ContainsKey(value))
                {
                    store.Add(value, new List<string>());
                }
                store[value].Add(id);
            }

            public void Remove(string id)
            {
                foreach (var list in store.Values)
                {
                    list.RemoveAll(item => item == id);
                }
                foreach (var pair in new Dictionary<T, List<string>>(store))
                {
                    if (pair.Value.Count == 0)
                    {
                        store.Remove(pair.Key);
                    }
                }
            }

            public IEnumerable<string> GetIdentifiers(T value)
            {
                List<string> list;
                return store.TryGetValue(value, out list) ? list : Enumerable.Empty<string>();
            }
        }
    }
}