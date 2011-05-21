using System;
using System.Collections.Generic;
using Jabber.Net.Server.Streams;

namespace Jabber.Net.Server.Listeners
{
    public class XmppListenersManager
    {
        private readonly IDictionary<Uri, IXmppListener> listeners = new Dictionary<Uri, IXmppListener>();
        private readonly XmppStreamsManager streamsManager;


        public bool IsListen
        {
            get;
            private set;
        }


        public XmppListenersManager(XmppStreamsManager streamsManager)
        {
            this.streamsManager = streamsManager;
        }


        public void AddListener(IXmppListener listener)
        {
            if (IsListen) throw new InvalidOperationException("ListenerManager in listen state.");
            if (listener == null) throw new ArgumentNullException("listener");

            listeners[listener.ListenUri] = listener;
        }

        public void RemoveListener(Uri listenUri)
        {
            if (IsListen) throw new InvalidOperationException("ListenerManager in listen state.");
            if (listenUri == null) throw new ArgumentNullException("listenUri");

            listeners.Remove(listenUri);
        }

        public void StartListen()
        {
            if (IsListen) return;

            foreach (var l in listeners.Values)
            {
                l.StartListen(Accept);
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


        private void Accept(XmppStream stream)
        {

        }
    }
}
