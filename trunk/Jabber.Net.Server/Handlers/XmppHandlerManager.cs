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


        public void ProcessElement(IXmppSender sender, XmppElement e)
        {
        }

        public void ProcessClose(IXmppSender sender, IEnumerable<XmppElement> notSended)
        {
        }

        public void ProcessError(IXmppSender sender, Exception error)
        {
            sender.Close();
        }
    }
}
