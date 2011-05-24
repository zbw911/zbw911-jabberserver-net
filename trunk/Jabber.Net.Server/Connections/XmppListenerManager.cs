using System;
using System.Collections.Generic;

namespace Jabber.Net.Server.Connections
{
    public class XmppListenerManager
    {
        private readonly IDictionary<Uri, IXmppListener> listeners = new Dictionary<Uri, IXmppListener>();
        private readonly XmppConnectionManager connectionManager;


        public bool IsListen
        {
            get;
            private set;
        }


        public XmppListenerManager(XmppConnectionManager connectionManager)
        {
            if (connectionManager == null) throw new ArgumentNullException("connectionManager");

            this.connectionManager = connectionManager;
        }


        public void AddListener(IXmppListener listener)
        {
            if (listener == null) throw new ArgumentNullException("listener");
            if (IsListen) throw new InvalidOperationException("ListenerManager in listen state.");

            listeners[listener.ListenUri] = listener;
        }

        public void RemoveListener(Uri listenUri)
        {
            if (listenUri == null) throw new ArgumentNullException("listenUri");
            if (IsListen) throw new InvalidOperationException("ListenerManager in listen state.");

            listeners.Remove(listenUri);
        }

        public void StartListen()
        {
            if (IsListen) return;

            foreach (var l in listeners.Values)
            {
                l.StartListen(connectionManager);
            }
            IsListen = true;
        }

        public void StopListen()
        {
            if (!IsListen) return;

            foreach (var l in listeners.Values)
            {
                l.StopListen();
            }
            IsListen = false;
        }
    }
}
