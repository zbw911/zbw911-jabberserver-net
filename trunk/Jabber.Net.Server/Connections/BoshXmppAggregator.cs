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
                connections.Add(rid, connection);
                var minrid = connections.Min(p => p.Key);
                current = connections[minrid];
            }
            return this;
        }

        public void BeginReceive(XmppHandlerManager handlerManager)
        {
            throw new NotSupportedException();
        }

        public void Send(Element element, Action<Element> onerror)
        {
            lock (locker)
            {
                if (!(element is BodyEnd))
                {
                    if (element is agsXMPP.protocol.client.Stream)
                    {
                        element = ((agsXMPP.protocol.client.Stream)element).Features;
                    }
                    buffer.Add(Tuple.Create(element, onerror));
                }
                if (!buffer.Any(t => t.Item1 is Body) || element is BodyEnd)
                {
                    var bodyTuple = buffer.FirstOrDefault(t => t.Item1 is Body);
                    var body = bodyTuple != null ? (Body)bodyTuple.Item1 : new Body();
                    foreach (var tuple in buffer)
                    {
                        if (!(tuple.Item1 is Body))
                        {
                            body.AddChild(tuple.Item1);
                        }
                    }
                    var onerrors = buffer
                        .Where(t => t.Item2 != null)
                        .Select<Tuple<Element, Action<Element>>, Action>(t => () => t.Item2(t.Item1))
                        .ToList();
                    Action<Element> commonOnError = _ => onerrors.ForEach(e => e());

                    buffer.Clear();
                    if (current != null)
                    {
                        current.Send(body, commonOnError);
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
                                }
                                else
                                {
                                    commonOnError(body);
                                }
                            }
                        };
                        TaskQueue.AddTask(Guid.NewGuid().ToString(), task, TimeSpan.FromSeconds(5));
                    }
                }
            }
        }

        public void Reset()
        {
        }

        public void Close()
        {

        }
    }
}
