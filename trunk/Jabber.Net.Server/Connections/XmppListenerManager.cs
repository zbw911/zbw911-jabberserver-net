using System;
using System.Collections.Generic;

namespace Jabber.Net.Server.Connections
{
    public class XmppListenerManager
    {
        private readonly IList<IXmppListener> listeners = new List<IXmppListener>();
        private bool listen = false;


        public void AddListener(IXmppListener listener)
        {
            Args.NotNull(listener, "listener");
            RequiresNotListen();

            listeners.Add(listener);
        }

        public void RemoveListener(IXmppListener listener)
        {
            Args.NotNull(listener, "listener");
            RequiresNotListen();

            listeners.Remove(listener);
        }


        public void StartListen(XmppConnectionManager connectionManager)
        {
            Args.NotNull(connectionManager, "connectionManager");
            RequiresNotListen();

            foreach (var listener in listeners)
            {
                try
                {
                    listener.StartListen(connectionManager);
                }
                catch (Exception error)
                {
                    Log.Error(error);
                }
            }
            listen = true;
        }

        public void StopListen()
        {
            if (listen)
            {
                foreach (var listener in listeners)
                {
                    try
                    {
                        listener.StopListen();
                    }
                    catch (Exception error)
                    {
                        Log.Error(error);
                    }
                }
                listen = false;
            }
        }


        private void RequiresNotListen()
        {
            Args.Requires<InvalidOperationException>(!listen, "ListenerManager in listen state.");
        }
    }
}
