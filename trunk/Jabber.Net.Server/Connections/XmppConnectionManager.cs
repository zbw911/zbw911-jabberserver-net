using System;
using System.Collections.Generic;
using System.Threading;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    public class XmppConnectionManager
    {
        private readonly IDictionary<string, XmppConnection> connections = new Dictionary<string, XmppConnection>(1000);
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private readonly XmppHandlerManager handlerManager;


        public XmppConnectionManager(XmppHandlerManager handlerManager)
        {
            Args.NotNull(handlerManager, "handlerManager");

            this.handlerManager = handlerManager;
        }


        public void AddConnection(IXmppConnection connection)
        {
            locker.EnterWriteLock();
            try
            {
                var wrapper = new XmppConnection(connection, this, handlerManager);
                connections.Add(wrapper.Id, wrapper);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void CloseConnection(string id)
        {
            XmppConnection connection;
            locker.EnterWriteLock();
            try
            {
                if (connections.TryGetValue(id, out connection))
                {
                    connections.Remove(id);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
