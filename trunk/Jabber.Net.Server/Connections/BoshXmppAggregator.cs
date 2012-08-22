using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol.extensions.bosh;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Connections
{
    class BoshXmppAggregator : IXmppConnection
    {
        private readonly object locker = new object();
        private readonly Dictionary<long, IXmppConnection> connections = new Dictionary<long, IXmppConnection>();
        private IXmppConnection current;
        private XmppHandlerManager handlerManager;

        private readonly TimeSpan waitTimeout;
        private readonly TimeSpan inactivityTimeout;
        private readonly TimeSpan sendTimeout;

        private readonly List<Tuple<Element, Action<Element>>> buffer = new List<Tuple<Element, Action<Element>>>();


        public string SessionId
        {
            get;
            set;
        }


        public BoshXmppAggregator(string sessionId, TimeSpan waitTimeout, TimeSpan inactivityTimeout, TimeSpan sendTimeout)
        {
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(sessionId), "Argument sessionId can not by empty.");

            this.SessionId = sessionId;
            this.waitTimeout = waitTimeout;
            this.inactivityTimeout = inactivityTimeout;
            this.sendTimeout = sendTimeout;
        }


        public IXmppConnection AddConnection(long rid, IXmppConnection connection)
        {
            Args.NotNull(connection, "connection");

            lock (locker)
            {
                TaskQueue.RemoveTask(SessionId);

                connections.Add(rid, connection);
                SetCurrent();
            }
            return this;
        }

        public void BeginReceive(XmppHandlerManager handlerManager)
        {
            Args.NotNull(handlerManager, "handlerManager");

            this.handlerManager = handlerManager;
        }

        public void Send(Element element, Action<Element> onerror)
        {
            // Send a single item or to accumulate a buffer until a bodyend.
            lock (locker)
            {
                if (!(element is BodyEnd))
                {
                    if (element is agsXMPP.protocol.client.Stream)
                    {
                        element = ((agsXMPP.protocol.client.Stream)element).Features;
                    }
                    buffer.Add(Tuple.Create(element, onerror));

                    if (buffer.Any(t => t.Item1 is Body))
                    {
                        return;
                    }
                }

                var elements = buffer.Select(t => t.Item1);
                var body = elements.FirstOrDefault(e => e is Body) as Body ?? new Body();
                foreach (var e in elements.Where(e => !(e is Body)))
                {
                    body.AddChild(e);
                }

                var onerrors = buffer
                    .Where(t => t.Item2 != null)
                    .Select(t => new Action(() => t.Item2(t.Item1)))
                    .ToList();
                Action<Element> commonOnError = _ => onerrors.ForEach(e => e());

                buffer.Clear();
                if (current != null)
                {
                    current.Send(body, commonOnError);
                    CloseCurrent();
                }
                else
                {
                    Action task = () =>
                    {
                        lock (locker)
                        {
                            if (current != null)
                            {
                                current.Send(body, commonOnError);
                                CloseCurrent();
                            }
                            else
                            {
                                commonOnError(body);
                            }
                        }
                    };
                    TaskQueue.AddTask(Guid.NewGuid().ToString(), task, sendTimeout);
                }
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            handlerManager.ProcessClose(this);
        }


        private void CloseCurrent()
        {
            lock (locker)
            {
                if (0 < connections.Count)
                {
                    SetCurrent();
                }
                else
                {
                    TaskQueue.AddTask(SessionId, () => Close(), inactivityTimeout);
                }
            }
        }

        private void SetCurrent()
        {
            if (current != null)
            {
                var minrid = connections.Min(p => p.Key);
                TaskQueue.RemoveTask(minrid.ToString());
                connections.Remove(minrid);
                current.Close();
                current = null;
            }
            if (0 < connections.Count)
            {
                var minrid = connections.Min(p => p.Key);
                current = connections[minrid];
                TaskQueue.AddTask(minrid.ToString(), () => CloseCurrent(), waitTimeout);
            }
        }
    }
}
