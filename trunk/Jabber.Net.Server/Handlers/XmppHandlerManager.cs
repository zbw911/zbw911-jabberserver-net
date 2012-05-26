using System;
using System.Collections.Generic;
using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        private readonly List<IXmppStreamHandler> streamHandlers;


        public XmppHandlerManager()
        {
            streamHandlers = new List<IXmppStreamHandler>(10);
        }


        public void AddHandler(IXmppStreamHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            streamHandlers.Add(handler);
        }

        public void RemoveHandler(IXmppStreamHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            streamHandlers.Remove(handler);
        }


        public void ProcessElement(IXmppConnection connection, XmppElement e)
        {
            if (e.IsStanza)
            {
                
            }
            else
            {
                foreach (var h in streamHandlers)
                {
                    if (e.Node.Namespace == h.Namespace)
                    {
                        var copy = (XmppElement)e.Clone();
                        h.ProcessElement(connection, e);
                    }
                }
            }
        }

        public void ProcessClose(IXmppConnection connection, IEnumerable<XmppElement> notSended)
        {
            foreach (var h in streamHandlers)
            {
                h.ProcessClose(connection, notSended);
            }
        }

        public void ProcessError(IXmppConnection connection, Exception error)
        {
            connection.Close();
        }
    }
}
